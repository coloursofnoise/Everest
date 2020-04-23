using FMOD.Studio;
using Microsoft.Xna.Framework;
using Monocle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Celeste.Mod.UI {
    public class OuiListSelect : Oui {
        private SearchMenu menu;

        private const float onScreenX = 960f;
        private const float offScreenX = 2880f;

        private float alpha = 0f;

        private Color searchBarColor;

        private List<TextMenuExt.IItemExt> items = new List<TextMenuExt.IItemExt>();
        private Dictionary<string, List<TextMenuExt.ButtonExt>> searchItems = new Dictionary<string, List<TextMenuExt.ButtonExt>>();

        public bool Searching;

        private string search = "";
        private string searchPrev = "";

        private TextMenu.Item searchTitle;

        private bool searchConsumedButton;

        private int itemCount;
        private int matchCount;

        private TextMenu.SubHeader resultHeader;

        private static TextMenuExt.SubHeaderExt perfectMatchHeader;

        private Action exit;
        private Action<string> onSelectOption;
        private Action<string> onSelectCategory;

        private class SearchMenu : Monocle.Entity {

            public bool leftFocused {
                get => leftMenu.Focused;
                set {
                    leftMenu.Focused = value;
                    rightMenu.Focused = !value;
                }
            }

            public TextMenu leftMenu;
            public TextMenu rightMenu;
            private float leftOffset;
            private float rightOffset;

            public int Selection {
                get => currentMenu.Selection;
                set => currentMenu.Selection = value;
            }

            public TextMenu currentMenu {
                get {
                    return leftFocused ? leftMenu : rightMenu;
                }
            }

            public bool Focused {
                get => leftMenu.Focused || rightMenu.Focused;
                set {
                    if (value) {
                        leftFocused = true;
                    } else {
                        leftMenu.Focused = false;
                        rightMenu.Focused = false;
                    }
                }
            }

            public SearchMenu(TextMenu leftMenu, TextMenu rightMenu) {
                Position = Vector2.Zero;
                this.leftMenu = leftMenu;
                this.rightMenu = rightMenu;
            }

            public override void Added(Scene scene) {
                base.Added(scene);
                rightMenu.InnerContent = TextMenu.InnerContentMode.TwoColumn;
                leftMenu.Position.X = Engine.Width / -4f;
                rightMenu.Position.X = Engine.Width / 4f;
                leftOffset = leftMenu.Position.X;
                rightOffset = rightMenu.Position.X;
                rightMenu.Focused = false;
                scene.Add(leftMenu);
                scene.Add(rightMenu);
            }

            public override void Removed(Scene scene) {
                scene.Remove(leftMenu);
                scene.Remove(rightMenu);
                base.Removed(scene);
            }

            public override void Update() {
                base.Update();
                leftMenu.Position.X = leftOffset + Position.X;
                rightMenu.Position.X = rightOffset + Position.X;
            }

        }

        public OuiListSelect Init<T>(Dictionary<string, List<TextMenuExt.ButtonExt>> options, Action<string> onSelectOption = null, Action<string> onSelectCategory = null) where T : Oui {
            searchItems = options;

            this.onSelectOption = onSelectOption;
            this.onSelectCategory = onSelectCategory;

            exit = () => Overworld.Goto<T>();

            return this;
        }

        public OuiListSelect Init<T>(Dictionary<string, string[]> options, Action<string> onSelectOption, Action<string> onSelectCategory = null) where T : Oui {
            foreach (KeyValuePair<string, string[]> set in options) {
                if (!searchItems.ContainsKey(set.Key))
                    searchItems[set.Key] = new List<TextMenuExt.ButtonExt>();
                foreach (string entry in set.Value)
                    searchItems[set.Key].Add(new TextMenuExt.ButtonExt(entry));
            }

            this.onSelectOption = onSelectOption;
            this.onSelectCategory = onSelectCategory;

            exit = () => Overworld.Goto<T>();

            return this;
        }

        public OuiListSelect Init<T>(List<TextMenuExt.ButtonExt> options, Action<string> onSelect = null) where T : Oui {
            searchItems[""] = options;

            onSelectOption = onSelect;

            exit = () => Overworld.Goto<T>();

            return this;
        }

        public OuiListSelect Init<T>(string[] list, Action<string> onSelect) where T : Oui {
            searchItems[""] = new List<TextMenuExt.ButtonExt>();
            foreach (string option in list) {
                searchItems[""].Add(new TextMenuExt.ButtonExt(option));
            }

            onSelectOption = onSelect;

            exit = () => Overworld.Goto<T>();

            return this;
        }

        private void clearSearch() {
            search = "";
            searchPrev = "";
        }

        private void cleanExit() {
            clearSearch();

            searchItems.Clear();
            items.Clear();

            Audio.Play(SFX.ui_main_button_back);
            exit();
        }

        public void OnTextInput(char c) {

            if (!Searching)
                return;

            if (c == (char) 13) {
                // Enter
                Scene.OnEndOfFrame += () => {
                    switchMenu();
                    if (items.Count >= 1) {
                        if (items.Count == 2 || matchCount == 1)
                            (items[1] as TextMenuExt.ButtonExt)?.OnPressed?.Invoke();

                        int index = menu.rightMenu.GetItems().FindIndex(item => item is TextMenuExt.ButtonExt button && button.Selectable && items.Contains(button));
                        if (index > 0) {
                            menu.rightMenu.Selection = index;
                            Audio.Play(SFX.ui_main_button_select);
                        }
                    }
                };

            } else if (c == (char) 8) {
                // Backspace - trim.
                if (search.Length > 0) {
                    search = search.Substring(0, search.Length - 1);
                    Audio.Play(SFX.ui_main_rename_entry_backspace);
                    goto ValidButton;
                } else {
                    if (Input.MenuCancel.Pressed) {
                        Audio.Play(SFX.ui_main_button_back);
                        switchMenu();
                        goto ValidButton;
                    }
                    return;
                }

            } else if (c == (char) 127) {
                // Delete - clear.
                if (search.Length > 0) {
                    clearSearch();
                    Audio.Play(SFX.ui_main_rename_entry_backspace);
                    goto ValidButton;
                } else {
                    Audio.Play(SFX.ui_main_button_invalid);
                    cleanExit();
                    goto ValidButton;
                }

            } else if (c == ' ') {
                // Space - append.
                if (search.Length > 0) {
                    if (ActiveFont.Measure(search + c + "_").X < 542)
                        search += c;
                }
                Audio.Play(SFX.ui_main_rename_entry_space);
                goto ValidButton;

            } else if (!char.IsControl(c)) {
                // Any other character - append.
                if (ActiveFont.FontSize.Characters.ContainsKey(c)) {
                    Audio.Play(SFX.ui_main_rename_entry_char);
                    if (ActiveFont.Measure(search + c + "_").X < 542)
                        search += c;
                    goto ValidButton;
                } else {
                    goto InvalidButton;
                }
            }

            return;

            ValidButton:
            searchConsumedButton = true;
            MInput.Disabled = true;
            MInput.UpdateNull();
            return;

            InvalidButton:
            Audio.Play(SFX.ui_main_button_invalid);
            return;
        }

        private SearchMenu CreateMenu(bool inGame, EventInstance snapshot) {
            menu = new SearchMenu(new TextMenu(), new TextMenu());
            items.Clear();

            menu.leftMenu.Add(searchTitle = new TextMenu.Header(Dialog.Clean("maplist_search")));

            menu.rightMenu.Add(resultHeader = new TextMenu.SubHeader(string.Format(itemCount == 1 ? Dialog.Get("maplist_results_singular") : Dialog.Get("maplist_results_plural"), itemCount)));

            ReloadItems();

            return menu;
        }

        private void ReloadItems() {
            itemCount = 0;
            matchCount = 0;

            foreach (TextMenu.Item item in items)
                menu.rightMenu.Remove(item);
            items.Clear();

            string name;
            string lastCategory = null;

            string[] searchHunks = search.Split(' ').Select(hunk => hunk.ToLower()).ToArray();
            bool matched = false;

            foreach (KeyValuePair<string, List<TextMenuExt.ButtonExt>> set in searchItems) {
                string category = set.Key;
                foreach (TextMenuExt.ButtonExt item in set.Value) {
                    string id = item.Label;
                    name = id.DialogCleanOrNull() ?? id.SpacedPascalCase();

                    List<string> matchTargets = new List<string> {
                        id,
                        category,
                        name
                    }.Select(text => text.ToLower()).ToList();

                    List<string> unmatchedHunks = searchHunks.ToList();

                    foreach (string hunk in searchHunks) {
                        if (matchTargets.Any(target => target.Contains(hunk)))
                            unmatchedHunks.Remove(hunk);
                    }

                    if (unmatchedHunks.Count > 0)
                        continue;

                    TextMenuExt.ButtonExt button = new TextMenuExt.ButtonExt(item.Label, item.Icon);

                    itemCount++;

                    button.Alpha = 0f;


                    if (name.ToLower() == search.ToLower()) {
                        if (!matched) {
                            perfectMatchHeader = new TextMenuExt.SubHeaderExt(Dialog.Clean("maplist_search_match"));
                            menu.rightMenu.Insert(0, perfectMatchHeader);
                            items.Insert(0, perfectMatchHeader);
                            matched = true;
                        }
                        matchCount++;
                        menu.rightMenu.Insert(1, button.Pressed(() => {
                            clearSearch();
                            item.OnPressed?.Invoke();
                            onSelectOption?.Invoke(button.Label);
                            onSelectCategory?.Invoke(category);
                            cleanExit();
                        }));
                        items.Insert(1, button);
                    } else {
                        if (lastCategory != category) {
                            lastCategory = category;
                            category = category.DialogCleanOrNull() ?? category.SpacedPascalCase();
                            TextMenuExt.SubHeaderExt levelSetHeader = new TextMenuExt.SubHeaderExt(category);
                            if (string.IsNullOrEmpty(category))
                                levelSetHeader.Visible = false;
                            levelSetHeader.Alpha = 0f;
                            menu.rightMenu.Add(levelSetHeader);
                            items.Add(levelSetHeader);
                        }

                        menu.rightMenu.Add(button.Pressed(() => {
                            clearSearch();
                            item.OnPressed?.Invoke();
                            onSelectOption?.Invoke(button.Label);
                            onSelectCategory?.Invoke(category);
                            cleanExit();
                        }));
                        items.Add(button);
                    }
                }
            }

            if (resultHeader != null) {
                resultHeader.Title = string.Format(itemCount == 1 ? Dialog.Get("maplist_results_singular") : Dialog.Get("maplist_results_plural"), itemCount);
            }

            // compute a delay so that options don't take more than a second to show up if many mods are installed.
            float delayBetweenOptions = 0.03f;
            if (items.Count > 0)
                delayBetweenOptions = Math.Min(0.03f, 1f / items.Count);

            // Do this afterwards as the menu has now properly updated its size.
            for (int i = 0; i < items.Count; i++)
                Add(new Coroutine(FadeIn(i, delayBetweenOptions, items[i])));

            if (menu.rightMenu.Height > menu.rightMenu.ScrollableMinSize) {
                menu.rightMenu.Position.Y = menu.rightMenu.ScrollTargetY;
            }

            // Don't allow pressing any buttons while searching
            foreach (TextMenu.Item item in items)
                item.Disabled = Searching;

            menu.rightMenu.Selection = itemCount > 1 ? 1 : 0;
        }

        private IEnumerator FadeIn(int i, float delayBetweenOptions, TextMenuExt.IItemExt item) {
            yield return delayBetweenOptions * i;
            float ease = 0f;

            Vector2 offset = item.Offset;

            for (float p = 0f; p < 1f; p += Engine.DeltaTime * 4f) {
                ease = Ease.CubeOut(p);
                item.Alpha = ease;
                item.Offset = offset + new Vector2(0f, 64f * (1f - ease));
                yield return null;
            }

            item.Alpha = 1f;
            item.Offset = offset;
        }

        private void ReloadMenu() {
            Vector2 position = Vector2.Zero;

            int selected = -1;
            bool leftSelected = true;
            if (menu != null) {
                position = menu.Position;
                leftSelected = menu.leftFocused;
                selected = menu.Selection;
                Scene.Remove(menu);
            }

            menu = CreateMenu(false, null);

            if (selected >= 0) {
                menu.Selection = selected;
                menu.Position = position;
            }
            IEnumerable<TextMenu> menus = Scene.Entities.OfType<TextMenu>();
            Scene.Remove(menus);
            Scene.Add(menu);
        }

        public override IEnumerator Enter(Oui from) {
            TextInput.OnInput += OnTextInput;

            searchBarColor = Color.DarkSlateGray;
            searchBarColor.A = 80;

            Searching = true;

            ReloadMenu();

            menu.rightMenu.MinWidth = menu.rightMenu.Width;

            menu.Visible = (Visible = true);
            menu.Focused = false;

            for (float p = 0f; p < 1f; p += Engine.DeltaTime * 4f) {
                menu.X = offScreenX + -1920f * Ease.CubeOut(p);
                alpha = Ease.CubeOut(p);
                yield return null;
            }

            menu.Focused = true;
        }

        public override IEnumerator Leave(Oui next) {
            TextInput.OnInput -= OnTextInput;

            MInput.Disabled = false;

            menu.Focused = false;

            Audio.Play(SFX.ui_main_whoosh_large_out);

            for (float p = 0f; p < 1f; p += Engine.DeltaTime * 4f) {
                menu.X = onScreenX + 1920f * Ease.CubeIn(p);
                alpha = 1f - Ease.CubeIn(p);
                yield return null;
            }

            menu.Visible = Visible = false;
            Searching = false;
            menu.RemoveSelf();
            menu = null;
        }

        private bool switchMenu() {
            bool nextIsLeft = !menu.leftFocused;
            if (nextIsLeft || items.Count > 1) {
                menu.leftFocused = nextIsLeft;
                Searching = nextIsLeft;
                MInput.Disabled = nextIsLeft;
                menu.currentMenu.Selection = nextIsLeft ? -1 : 2;
                return true;
            }
            return false;
        }

        public override void Update() {
            if (Searching) {
                MInput.Disabled = searchConsumedButton;
            }
            searchConsumedButton = false;

            if (menu != null && menu.Focused && Selected) {
                if (search != searchPrev) {
                    ReloadItems();
                    searchPrev = search;
                }

                if (Input.MenuCancel.Pressed || Input.Pause.Pressed || Input.ESC.Pressed) {
                    if (Searching && search != "") {
                        if (!switchMenu()) {
                            cleanExit();
                        }
                    } else {
                        cleanExit();
                    }
                }

                if (Input.MenuRight.Pressed) {
                    if (!menu.leftFocused)
                        return;
                    Audio.Play(SFX.ui_main_button_toggle_on);
                    switchMenu();
                }

                if (Input.MenuLeft.Pressed) {
                    if (menu.leftFocused)
                        return;
                    Audio.Play(SFX.ui_main_button_toggle_off);
                    switchMenu();
                }

            }

            MInput.Disabled = Searching;

            base.Update();

            if (Searching) {
                // Otherwise spacebar will turn on free cam while searching
                ((patch_MountainRenderer)Overworld.Mountain).SetFreeCam(false);
            }

            // Don't allow pressing any buttons while searching
            if (menu != null)
                foreach (TextMenu.Item item in menu.rightMenu.GetItems())
                    item.Disabled = Searching;
        }

        public override void Render() {
            if (alpha > 0f)
                Draw.Rect(-10f, -10f, 1940f, 1100f, Color.Black * alpha * 0.4f);

            TextMenu leftMenu = menu.leftMenu;
            // Draw the search
            if (searchTitle != null) {
                Vector2 value = leftMenu.Position + leftMenu.Justify * new Vector2(leftMenu.Width, leftMenu.Height);
                Vector2 pos = new Vector2(value.X - 200f, value.Y + leftMenu.GetYOffsetOf(searchTitle) + 1f);
                Draw.Rect(pos + new Vector2(-8f, 32f), 416, (int) (ActiveFont.HeightOf("l" + search) + 8) * -1, searchBarColor);
                ActiveFont.DrawOutline(search + (Searching ? "_" : ""), pos, new Vector2(0f, 0.5f), Vector2.One * 0.75f, Color.White * leftMenu.Alpha, 2f, Color.Black * (leftMenu.Alpha * leftMenu.Alpha * leftMenu.Alpha));
            }

            base.Render();
        }
    }
}