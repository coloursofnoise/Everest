﻿using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.Utils;
using MonoMod.InlineRT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonoMod.Cil;

namespace MonoMod {
    /// <summary>
    /// Proxy any System.IO.File.* calls inside the method via Celeste.Mod.Helpers.FileProxy.*
    /// </summary>
    [MonoModCustomMethodAttribute("ProxyFileCalls")]
    class ProxyFileCallsAttribute : Attribute { }

    /// <summary>
    /// Check for ldstr "Corrupted Level Data" and pop the throw after that.
    /// Also manually execute ProxyFileCalls rule.
    /// Also includes a patch for the strawberry tracker.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchMapDataLoader")]
    class PatchMapDataLoaderAttribute : Attribute { }

    /// <summary>
    /// A patch for the strawberry tracker, allowing all registered modded berries to be detected.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLevelDataBerryTracker")]
    class PatchLevelDataBerryTracker : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized level loading method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLevelLoader")]
    class PatchLevelLoaderAttribute : Attribute { }

    /// <summary>
    /// A patch for Strawberry that takes into account that some modded strawberries may not allow standard collection rules.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchStrawberryTrainCollectionOrder")]
    class PatchStrawberryTrainCollectionOrder : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized backdrop parsing method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchBackdropParser")]
    class PatchBackdropParserAttribute : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized level updating method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLevelUpdate")]
    class PatchLevelUpdateAttribute : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized level rendering method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLevelRender")]
    class PatchLevelRenderAttribute : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized level loading thread method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLevelLoaderThread")]
    class PatchLevelLoaderThreadAttribute : Attribute { }

    /// <summary>
    /// Patch the Godzilla-sized level transition method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchTransitionRoutine")]
    class PatchTransitionRoutineAttribute : Attribute { }

    /// <summary>
    /// Find ldfld Engine::Version + ToString. Pop ToString result, call Everest::get_VersionCelesteString
    /// </summary>
    [MonoModCustomMethodAttribute("PatchErrorLogWrite")]
    class PatchErrorLogWriteAttribute : Attribute { }

    /// <summary>
    /// Patch the heart gem collection routine instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchHeartGemCollectRoutine")]
    class PatchHeartGemCollectRoutineAttribute : Attribute { }

    /// <summary>
    /// Patch the Badeline chase routine instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchBadelineChaseRoutine")]
    class PatchBadelineChaseRoutineAttribute : Attribute { }

    /// <summary>
    /// Patch the Badeline boss OnPlayer method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchBadelineBossOnPlayer")]
    class PatchBadelineBossOnPlayerAttribute : Attribute { }

    /// <summary>
    /// Patch the Cloud.Added method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchCloudAdded")]
    class PatchCloudAddedAttribute : Attribute { }

    /// <summary>
    /// Patch the RainFG.Render method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchRainFGRender")]
    class PatchRainFGRenderAttribute : Attribute { }

    /// <summary>
    /// Patch the Dialog.Load method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchDialogLoader")]
    class PatchDialogLoaderAttribute : Attribute { }

    /// <summary>
    /// Patch the Language.LoadTxt method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLoadLanguage")]
    class PatchLoadLanguageAttribute : Attribute { }

    /// <summary>
    /// Automatically fill InitMMSharedData based on the current patch flags.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchInitMMSharedData")]
    class PatchInitMMSharedDataAttribute : Attribute { }

    /// <summary>
    /// Slap a ldfld completeMeta right before newobj AreaComplete
    /// </summary>
    [MonoModCustomMethodAttribute("RegisterLevelExitRoutine")]
    class PatchLevelExitRoutineAttribute : Attribute { }

    /// <summary>
    /// Slap a MapMetaCompleteScreen param at the end of the constructor and ldarg it right before newobj CompleteRenderer
    /// </summary>
    [MonoModCustomMethodAttribute("RegisterAreaCompleteCtor")]
    class PatchAreaCompleteCtorAttribute : Attribute { }

    /// <summary>
    /// Patch the GameLoader.IntroRoutine method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchGameLoaderIntroRoutine")]
    class PatchGameLoaderIntroRoutineAttribute : Attribute { }

    /// <summary>
    /// Patch the UserIO.SaveRoutine method instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchSaveRoutine")]
    class PatchSaveRoutineAttribute : Attribute { }

    /// <summary>
    /// Patch the orig_Update method in Player instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchPlayerOrigUpdate")]
    class PatchPlayerOrigUpdateAttribute : Attribute { }

    /// <summary>
    /// Patch the SwapRoutine method in OuiChapterPanel instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchChapterPanelSwapRoutine")]
    class PatchChapterPanelSwapRoutineAttribute : Attribute { }

    /// <summary>
    /// Patch the Strawberry class to tack on the IStrawberry interface for the StrawberryRegistry
    /// </summary>
    [MonoModCustomAttribute("PatchStrawberryInterface")]
    class PatchStrawberryInterfaceAttribute : Attribute { }

    /// <summary>
    /// Helper for patching methods force-implemented by an interface
    /// </summary>
    [MonoModCustomMethodAttribute("PatchInterface")]
    class PatchInterfaceAttribute : Attribute { };

    /// <summary>
    /// IL-patch the Render method for file select slots instead of reimplementing it,
    /// to un-hardcode stamps.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchFileSelectSlotRender")]
    class PatchFileSelectSlotRenderAttribute : Attribute { };

    /// <summary>
    /// Take out the "strawberry" equality check and replace it with a call to StrawberryRegistry.TrackableContains
    /// to include registered mod berries as well.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchTrackableStrawberryCheck")]
    class PatchTrackableStrawberryCheckAttribute : Attribute { };

    /// <summary>
    /// Patch the pathfinder debug rendering to make it aware of the array size being unhardcoded.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchPathfinderRender")]
    class PatchPathfinderRenderAttribute : Attribute { };

    /// <summary>
    /// Patch references to TotalHeartGems to refer to TotalHeartGemsInVanilla instead.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchTotalHeartGemChecks")]
    class PatchTotalHeartGemChecksAttribute : Attribute { };

    /// <summary>
    /// Same as above, but for references in routines.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchTotalHeartGemChecksInRoutine")]
    class PatchTotalHeartGemChecksInRoutineAttribute : Attribute { };

    /// <summary>
    /// Patch a reference to TotalHeartGems in the OuiJournalGlobal constructor to unharcode the check for golden berry unlock.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchOuiJournalStatsHeartGemCheck")]
    class PatchOuiJournalStatsHeartGemCheckAttribute : Attribute { };

    /// <summary>
    /// Makes the annotated method public.
    /// </summary>
    [MonoModCustomMethodAttribute("MakeMethodPublic")]
    class MakeMethodPublicAttribute : Attribute { };

    /// <summary>
    /// Patches the CrystalStaticSpinner.AddSprites method to make it more efficient.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchSpinnerCreateSprites")]
    class PatchSpinnerCreateSpritesAttribute : Attribute { };

    /// <summary>
    /// Patches the checks for OuiAssistMode to include a check for OuiFileSelectSlot.ISubmenu as well.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchOuiFileSelectSubmenuChecks")]
    class PatchOuiFileSelectSubmenuChecksAttribute : Attribute { };

    /// <summary>
    /// Patches the Fonts.Prepare method to also include custom fonts.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchFontsPrepare")]
    class PatchFontsPrepareAttribute : Attribute { };

    /// <summary>
    /// Make the marked method the new entry point.
    /// </summary>
    [MonoModCustomMethodAttribute("MakeEntryPoint")]
    class MakeEntryPointAttribute : Attribute { };

    /// <summary>
    /// Patch the original Celeste entry point instead of reimplementing it in Everest.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchCelesteMain")]
    class PatchCelesteMainAttribute : Attribute { };

    /// <summary>
    /// Removes the [Command] attribute from the matching vanilla method in Celeste.Commands.
    /// </summary>
    [MonoModCustomMethodAttribute("RemoveCommandAttributeFromVanillaLoadMethod")]
    class RemoveCommandAttributeFromVanillaLoadMethodAttribute : Attribute { };

    /// <summary>
    /// Patch the fake heart color to make it customizable.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchFakeHeartColor")]
    class PatchFakeHeartColorAttribute : Attribute { };

    /// <summary>
    /// Patch the file naming rendering to hide the "switch between katakana and hiragana" prompt when the menu is not focused.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchOuiFileNamingRendering")]
    class PatchOuiFileNamingRenderingAttribute : Attribute { };

    /// <summary>
    /// Include the option to use Y range of trigger nodes.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchRumbleTriggerAwake")]
    class PatchRumbleTriggerAwakeAttribute : Attribute { };

    /// <summary>
    /// Include check for custom events.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchEventTriggerOnEnter")]
    class PatchEventTriggerOnEnterAttribute : Attribute { };

    /// <summary>
    /// Modify collision to make it customizable.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchWaterUpdate")]
    class PatchWaterUpdateAttribute : Attribute { };

    /// <summary>
    /// Add custom dialog to fake hearts.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchFakeHeartDialog")]
    class PatchFakeHeartDialogAttribute : Attribute { };

    /// <summary>
    /// Include checks for manual triggering.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchIntroCrusherSequence")]
    class PatchIntroCrusherSequenceAttribute : Attribute { };

    /// <summary>
    /// Patches the unselected color in TextMenu.Option to make it customizable.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchTextMenuOptionColor")]
    class PatchTextMenuOptionColorAttribute : Attribute { };

    /// <summary>
    /// Patches chapter panel rendering to allow for custom chapter cards.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchOuiChapterPanelRender")]
    class PatchOuiChapterPanelRenderAttribute : Attribute { };

    /// <summary>
    /// Patches GoldenBlocks to disable static movers if the block is disabled.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchGoldenBlockStaticMovers")]
    class PatchGoldenBlockStaticMoversAttribute : Attribute { };

    /// <summary>
    /// Don't remove TalkComponent even watchtower collide solid, so that watchtower can be hidden behind Solid.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchLookoutUpdate")]
    class PatchLookoutUpdateAttribute : Attribute { };

    /// <summary>
    /// Un-hardcode the range of the "Scared" decals.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchDecalUpdate")]
    class PatchDecalUpdateAttribute : Attribute { };

    /// <summary>
    /// Patches LevelExit.Begin to make the endscreen music customizable.
    /// </summary>
    [MonoModCustomMethodAttribute("PatchAreaCompleteMusic")]
    class PatchAreaCompleteMusicAttribute : Attribute { };

    static class MonoModRules {

        static bool IsCeleste;

        static bool FMODStub = false;

        static TypeDefinition Celeste;

        static TypeDefinition Everest;
        static MethodDefinition m_Everest_get_VersionCelesteString;

        static TypeDefinition Level;

        static TypeDefinition StrawberryRegistry;
        static InterfaceImplementation IStrawberry;

        static TypeDefinition SaveData;

        static TypeDefinition FileProxy;
        static TypeDefinition DirectoryProxy;
        static IDictionary<string, MethodDefinition> FileProxyCache = new Dictionary<string, MethodDefinition>();
        static IDictionary<string, MethodDefinition> DirectoryProxyCache = new Dictionary<string, MethodDefinition>();

        static List<MethodDefinition> LevelExitRoutines = new List<MethodDefinition>();
        static List<MethodDefinition> AreaCompleteCtors = new List<MethodDefinition>();

        static MonoModRules() {
            // Note: It may actually be too late to set this to false.
            MonoModRule.Modder.MissingDependencyThrow = false;

            FMODStub = Environment.GetEnvironmentVariable("EVEREST_FMOD_STUB") == "1";
            MonoModRule.Flag.Set("FMODStub", FMODStub);

            bool isFNA = false;
            bool isSteamworks = false;
            foreach (AssemblyNameReference name in MonoModRule.Modder.Module.AssemblyReferences) {
                if (name.Name.Contains("FNA"))
                    isFNA = true;
                else if (name.Name.Contains("Steamworks"))
                    isSteamworks = true;
            }
            MonoModRule.Flag.Set("FNA", isFNA);
            MonoModRule.Flag.Set("XNA", !isFNA);
            MonoModRule.Flag.Set("Steamworks", isSteamworks);
            MonoModRule.Flag.Set("NoLauncher", !isSteamworks);

            if (Celeste == null)
                Celeste = MonoModRule.Modder.FindType("Celeste.Celeste")?.Resolve();
            if (Celeste == null)
                return;
            IsCeleste = Celeste.Scope == MonoModRule.Modder.Module;

            if (IsCeleste) {
                MonoModRule.Modder.PostProcessors += PostProcessor;
            }

            // Get version - used to set any MonoMod flags.

            string versionString = null;
            int[] versionInts = null;
            // Find Celeste .ctor (luckily only has one)
            MethodDefinition c_Celeste = Celeste.FindMethod(".ctor", true);
            if (c_Celeste != null && c_Celeste.HasBody) {
                Mono.Collections.Generic.Collection<Instruction> instrs = c_Celeste.Body.Instructions;
                for (int instri = 0; instri < instrs.Count; instri++) {
                    Instruction instr = instrs[instri];
                    MethodReference c_Version = instr.Operand as MethodReference;
                    if (instr.OpCode != OpCodes.Newobj || c_Version?.DeclaringType?.FullName != "System.Version")
                        continue;

                    // We're constructing a System.Version - check if all parameters are of type int.
                    bool c_Version_intsOnly = true;
                    foreach (ParameterReference param in c_Version.Parameters)
                        if (param.ParameterType.MetadataType != MetadataType.Int32) {
                            c_Version_intsOnly = false;
                            break;
                        }

                    if (c_Version_intsOnly) {
                        // Assume that ldc.i4* instructions are right before the newobj.
                        versionInts = new int[c_Version.Parameters.Count];
                        for (int i = -versionInts.Length; i < 0; i++)
                            versionInts[i + versionInts.Length] = instrs[i + instri].GetInt();
                    }

                    if (c_Version.Parameters.Count == 1 && c_Version.Parameters[0].ParameterType.MetadataType == MetadataType.String) {
                        // Assume that a ldstr is right before the newobj.
                        versionString = instrs[instri - 1].Operand as string;
                    }

                    // Don't check any other instructions.
                    break;
                }
            }

            // Construct the version from our gathered data.
            Version version = new Version();
            if (versionString != null) {
                version = new Version(versionString);
            }
            if (versionInts == null || versionInts.Length == 0) {
                // ???
            } else if (versionInts.Length == 2) {
                version = new Version(versionInts[0], versionInts[1]);
            } else if (versionInts.Length == 3) {
                version = new Version(versionInts[0], versionInts[1], versionInts[2]);
            } else if (versionInts.Length == 4) {
                version = new Version(versionInts[0], versionInts[1], versionInts[2], versionInts[3]);
            }

            // Check if Celeste version is supported
            Version versionMin = new Version(1, 3, 1, 2);
            if (version.Major == 0)
                version = versionMin;
            if (version < versionMin)
                throw new Exception($"Unsupported version of Celeste: {version}");

            if (IsCeleste) {
                // Ensure that Celeste assembly is not already modded
                // (https://github.com/MonoMod/MonoMod#how-can-i-check-if-my-assembly-has-been-modded)
                if (MonoModRule.Modder.FindType("MonoMod.WasHere") != null)
                    throw new Exception("This version of Celeste is already modded. You need a clean install of Celeste to mod it.");
            }


            // Set up flags.

            bool isWindows = PlatformHelper.Is(Platform.Windows);
            MonoModRule.Flag.Set("OS:Windows", isWindows);
            MonoModRule.Flag.Set("OS:NotWindows", !isWindows);

            MonoModRule.Flag.Set("Has:BirdTutorialGuiButtonPromptEnum", MonoModRule.Modder.FindType("Celeste.BirdTutorialGui/ButtonPrompt")?.SafeResolve() != null);
            MonoModRule.Flag.Set("Fill:CompleteRendererImageLayerScale", MonoModRule.Modder.FindType("Celeste.CompleteRenderer/ImageLayer").Resolve().FindField("Scale") == null);

            TypeDefinition t_Input = MonoModRule.Modder.FindType("Celeste.Input").Resolve();
            MethodDefinition m_GuiInputController = t_Input.FindMethod("GuiInputController");
            MonoModRule.Flag.Set("V1:GuiInputController", m_GuiInputController.Parameters.Count == 0);
            MonoModRule.Flag.Set("V2:GuiInputController", m_GuiInputController.Parameters.Count == 1);
        }

        public static void ProxyFileCalls(MethodDefinition method, CustomAttribute attrib) {
            if (FileProxy == null)
                FileProxy = MonoModRule.Modder.FindType("Celeste.Mod.Helpers.FileProxy")?.Resolve();

            if (DirectoryProxy == null)
                DirectoryProxy = MonoModRule.Modder.FindType("Celeste.Mod.Helpers.DirectoryProxy")?.Resolve();

            foreach (Instruction instr in method.Body.Instructions) {
                // System.IO.File.* calls are always static calls.
                if (instr.OpCode != OpCodes.Call)
                    continue;

                // We only want to replace System.IO.File.* calls.
                MethodReference calling = instr.Operand as MethodReference;
                MethodDefinition replacement;

                if (calling?.DeclaringType?.FullName == "System.IO.File") {
                    if (!FileProxyCache.TryGetValue(calling.Name, out replacement))
                        FileProxyCache[calling.GetID(withType: false)] = replacement = FileProxy.FindMethod(calling.GetID(withType: false));

                } else if (calling?.DeclaringType?.FullName == "System.IO.Directory") {
                    if (!DirectoryProxyCache.TryGetValue(calling.Name, out replacement))
                        DirectoryProxyCache[calling.GetID(withType: false)] = replacement = DirectoryProxy.FindMethod(calling.GetID(withType: false));

                } else {
                    continue;
                }

                if (replacement == null)
                    continue; // We haven't got any replacement.

                // Replace the called method with our replacement.
                instr.Operand = replacement;
            }

        }

        public static void PatchMapDataLoader(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));

            ProxyFileCalls(method, attrib);

            MethodDefinition m_Process = method.DeclaringType.FindMethod("Celeste.BinaryPacker/Element _Process(Celeste.BinaryPacker/Element,Celeste.MapData)");
            MethodDefinition m_GrowAndGet = method.DeclaringType.FindMethod("Celeste.EntityData _GrowAndGet(Celeste.EntityData[0...,0...]&,System.Int32,System.Int32)");

            bool pop = false;
            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.MatchLdstr("Corrupted Level Data"))
                    pop = true;

                if (pop && instr.OpCode == OpCodes.Throw) {
                    instr.OpCode = OpCodes.Pop;
                    pop = false;
                }

                if (instr.MatchCall("Celeste.BinaryPacker", "FromBinary")) {
                    instri++;

                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Call, m_Process));
                }

                if (instri > 2 &&
                    instrs[instri - 3].MatchLdfld("Celeste.ModeProperties", "StrawberriesByCheckpoint") &&
                    instr.MatchCallvirt("Celeste.EntityData[0...,0...]", "Get")
                ) {
                    instrs[instri - 3].OpCode = OpCodes.Ldflda;
                    instr.OpCode = OpCodes.Call;
                    instr.Operand = m_GrowAndGet;
                    instri++;
                }
            }

        }

        public static void PatchTrackableStrawberryCheck(MethodDefinition method, CustomAttribute attrib) {
            if (StrawberryRegistry == null)
                StrawberryRegistry = MonoModRule.Modder.FindType("Celeste.Mod.StrawberryRegistry")?.Resolve();

            MethodDefinition m_TrackableContains = StrawberryRegistry.FindMethod("System.Boolean TrackableContains(System.String)");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.MatchLdstr("strawberry")) {
                    instr.OpCode = OpCodes.Nop;
                    instri++;
                    instrs[instri].OpCode = OpCodes.Call;
                    instrs[instri].Operand = m_TrackableContains;
                }
            }
        }

        public static void PatchLevelDataBerryTracker(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));

            if (StrawberryRegistry == null)
                StrawberryRegistry = MonoModRule.Modder.FindType("Celeste.Mod.StrawberryRegistry")?.Resolve();

            MethodDefinition m_TrackableContains = StrawberryRegistry.FindMethod("System.Boolean TrackableContains(Celeste.BinaryPacker/Element)");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                /*
                   we found

                   IL_08BA: ldloc.s   V_14
                   IL_08BC: ldfld     string Celeste.BinaryPacker/Element::Name
                   IL_08C1: ldstr     "strawberry"      <-- YOU ARE HERE
                   IL_08C6: call      bool [mscorlib]System.String::op_Equality(string, string)
                   IL_08CB: brtrue.s  IL_08E0
                */

                // Strawberry tracker adjustments
                if (instr.MatchLdstr("strawberry")) {

                    instr.OpCode = OpCodes.Nop;
                    instrs[instri - 1].OpCode = OpCodes.Nop;
                    instrs[instri + 1].Operand = m_TrackableContains;
                    instri++;
                }
            }
        }

        public static void PatchStrawberryTrainCollectionOrder(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));

            if (StrawberryRegistry == null)
                StrawberryRegistry = MonoModRule.Modder.FindType("Celeste.Mod.StrawberryRegistry")?.Resolve();

            MethodDefinition m_IsFirst = StrawberryRegistry.FindMethod("System.Boolean IsFirstStrawberry(Monocle.Entity)");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                // Rip out the vanilla code call and replace it with vanilla-considerate code
                if (instr.MatchCallvirt("Celeste.Strawberry", "get_IsFirstStrawberry")) {
                    instr.OpCode = OpCodes.Call;
                    instr.Operand = m_IsFirst;
                    instri++;
                }
            }
        }

        public static void PatchLevelLoader(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));
            // We also need to do special work in the cctor.
            MethodDefinition m_cctor = method.DeclaringType.FindMethod(".cctor");

            MethodDefinition m_LoadNewPlayer = method.DeclaringType.FindMethod("Celeste.Player LoadNewPlayer(Microsoft.Xna.Framework.Vector2,Celeste.PlayerSpriteMode)");
            MethodDefinition m_LoadCustomEntity = method.DeclaringType.FindMethod("System.Boolean LoadCustomEntity(Celeste.EntityData,Celeste.Level)");
            
            FieldDefinition f_LoadStrings = method.DeclaringType.FindField("_LoadStrings");

            Mono.Collections.Generic.Collection<Instruction> cctor_instrs = m_cctor.Body.Instructions;
            ILProcessor cctor_il = m_cctor.Body.GetILProcessor();

            // Remove cctor ret for simplicity. Re-add later.
            cctor_instrs.RemoveAt(cctor_instrs.Count - 1);

            TypeDefinition td_LoadStrings = f_LoadStrings.FieldType.Resolve();
            MethodReference m_LoadStrings_Add = MonoModRule.Modder.Module.ImportReference(td_LoadStrings.FindMethod("Add"));
            m_LoadStrings_Add.DeclaringType = f_LoadStrings.FieldType;
            MethodReference m_LoadStrings_ctor = MonoModRule.Modder.Module.ImportReference(td_LoadStrings.FindMethod("System.Void .ctor()"));
            m_LoadStrings_ctor.DeclaringType = f_LoadStrings.FieldType;
            cctor_il.Emit(OpCodes.Newobj, m_LoadStrings_ctor);

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.MatchNewobj("Celeste.Player")) {
                    instr.OpCode = OpCodes.Call;
                    instr.Operand = m_LoadNewPlayer;
                }

                /* We expect something similar enough to the following:
                ldwhatever the entityData into stack
                ldfld     string Celeste.EntityData::Name // We're here
                stloc*
                ldloc*
                call      uint32 '<PrivateImplementationDetails>'::ComputeStringHash(string)

                Note that MonoMod requires the full type names (System.UInt32 instead of uint32) and skips escaping 's
                */

                if (instri > 0 &&
                        instri < instrs.Count - 4 &&
                        instr.MatchLdfld("Celeste.EntityData", "Name") &&
                        instrs[instri + 1].MatchStloc(out int _) &&
                        instrs[instri + 2].MatchLdloc(out int _) &&
                        instrs[instri + 3].MatchCall("<PrivateImplementationDetails>", "ComputeStringHash")
                    ) {
                    // Insert a call to our own entity handler here.
                    // If it returns true, replace the name with ""

                    // Avoid loading entityData again.
                    // Instead, duplicate already loaded existing value.
                    instrs.Insert(instri++, il.Create(OpCodes.Dup));
                    // Load "this" onto stack - we're too lazy to shift this to the beginning of the stack.
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));

                    // Call our static custom entity handler.
                    instrs.Insert(instri++, il.Create(OpCodes.Call, m_LoadCustomEntity));

                    // If we returned false, branch to ldfld. We still have the entity name on stack.
                    // This basically translates to if (result) { pop; ldstr ""; }; ldfld ...
                    instrs.Insert(instri++, il.Create(OpCodes.Brfalse_S, instrs[instri]));
                    // Otherwise, pop the entityData, load "" and jump to stloc to skip any original entity handler.
                    instrs.Insert(instri++, il.Create(OpCodes.Pop));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldstr, ""));
                    instrs.Insert(instri++, il.Create(OpCodes.Br_S, instrs[instri + 1]));
                }

                if (instr.OpCode == OpCodes.Ldstr) {
                    cctor_il.Emit(OpCodes.Dup);
                    cctor_il.Emit(OpCodes.Ldstr, instr.Operand);
                    cctor_il.Emit(OpCodes.Callvirt, m_LoadStrings_Add);
                    cctor_il.Emit(OpCodes.Pop); // HashSet.Add returns a bool.
                }

            }

            cctor_il.Emit(OpCodes.Stsfld, f_LoadStrings);
            cctor_il.Emit(OpCodes.Ret);
        }

        public static void PatchBackdropParser(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_LoadCustomBackdrop = context.Method.DeclaringType.FindMethod("Celeste.Backdrop LoadCustomBackdrop(Celeste.BinaryPacker/Element,Celeste.BinaryPacker/Element,Celeste.MapData)");

            ILCursor cursor = new ILCursor(context);
            // Remove soon-to-be-unneeded instructions
            cursor.RemoveRange(2);

            // Load custom backdrop at the beginning of the method.
            // If it's been loaded, skip to backdrop setup.
            cursor.Emit(OpCodes.Ldarg_1);
            cursor.Emit(OpCodes.Ldarg_2);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_LoadCustomBackdrop);
            cursor.Emit(OpCodes.Stloc_0);
            cursor.Emit(OpCodes.Ldloc_0);

            // Get the branch target for if a custom backdrop is found
            cursor.FindNext(out ILCursor[] cursors, instr => instr.MatchLdstr("tag"));
            Instruction branchCustomToSetup = cursors[0].Prev;
            cursor.Emit(OpCodes.Brtrue, branchCustomToSetup);
        }

        public static void PatchLevelUpdate(ILContext context, CustomAttribute attrib) {
            /* We expect something similar enough to the following:
            call class Monocle.MInput/KeyboardData Monocle.MInput::get_Keyboard() // We're here
            ldc.i4.s 9
            callvirt instance bool Monocle.MInput/KeyboardData::Pressed(valuetype [FNA]Microsoft.Xna.Framework.Input.Keys) 

            We're replacing 
            MInput.Keyboard.Pressed(Keys.Tab) 
            with 
            false
            */

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchCall("Monocle.MInput", "get_Keyboard"),
                instr => instr.GetIntOrNull() == 9,
                instr => instr.MatchCallvirt("Monocle.MInput/KeyboardData", "Pressed"));
            // Remove the offending instructions, and replace them with 0 (false)
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Ldc_I4_0);
        }

        public static void PatchLevelRender(ILContext context, CustomAttribute attrib) {
            FieldDefinition f_SubHudRenderer = context.Method.DeclaringType.FindField("SubHudRenderer");

            /* We expect something similar enough to the following (different on SteamFNA but still works):
            if (!this.Paused || !this.PauseMainMenuOpen || !Input.MenuJournal.Check || !this.AllowHudHide)
	        {
		        this.HudRenderer.Render(this);
	        }
            and we want to prepend it with:
            this.SubHudRenderer.Render(this);
            */

            ILCursor cursor = new ILCursor(context);
            // Make use of the pre-existing Ldarg_0
            cursor.GotoNext(instr => instr.MatchLdfld("Monocle.Scene", "Paused"));
            // Retrieve a reference to Renderer.Render(Scene) from the following this.HudRenderer.Render(this)
            cursor.FindNext(out ILCursor[] render, instr => instr.MatchCallvirt("Monocle.Renderer", "Render"));
            MethodReference m_Renderer_Render = (MethodReference) render[0].Next.Operand;

            cursor.Emit(OpCodes.Ldfld, f_SubHudRenderer);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Callvirt, m_Renderer_Render);
            // Re-add the Ldarg_0 we cannibalized
            cursor.Emit(OpCodes.Ldarg_0);
        }

        public static void PatchLevelLoaderThread(ILContext context, CustomAttribute attrib) {
            if (Level == null)
                Level = MonoModRule.Modder.FindType("Celeste.Level").Resolve();

            FieldDefinition f_SubHudRenderer = Level.FindField("SubHudRenderer");
            MethodDefinition ctor_SubHudRenderer = f_SubHudRenderer.FieldType.Resolve().FindMethod("System.Void .ctor()");

            // Add a local variable we'll use to store a SubHudRenderer object temporarily
            VariableDefinition loc_SubHudRenderer_0 = new VariableDefinition(f_SubHudRenderer.FieldType);
            context.Body.Variables.Add(loc_SubHudRenderer_0);

            /*
            We just want to add
            this.Level.Add(this.Level.SubHudRenderer = new SubHudRenderer());
            before
            this.Level.Add(this.Level.HudRenderer = new HudRenderer());
             */

            ILCursor cursor = new ILCursor(context);
            // Got to the point just before we want to add our code, making use of the Level objects loaded for the HudRenderer
            cursor.GotoNext(instr => instr.MatchNewobj("Celeste.HudRenderer"));
            // Retrieve methods we want to use from around the target instruction
            MethodReference m_LevelLoader_get_Level = (MethodReference) cursor.Prev.Operand;
            cursor.FindNext(out ILCursor[] cursors, instr => instr.MatchCallvirt("Monocle.Scene", "Add"));
            MethodReference m_Scene_Add = (MethodReference) cursors[0].Next.Operand;

            // Load the new renderer onto the stack and duplicate it.
            cursor.Emit(OpCodes.Newobj, ctor_SubHudRenderer);
            cursor.Emit(OpCodes.Dup);
            // Store one copy in a local variable for later
            cursor.Emit(OpCodes.Stloc_S, loc_SubHudRenderer_0);
            // Store the other copy in its field
            cursor.Emit(OpCodes.Stfld, f_SubHudRenderer);
            // Load the first copy back onto the stack
            cursor.Emit(OpCodes.Ldloc_S, loc_SubHudRenderer_0);
            // And add it to the scene
            cursor.Emit(OpCodes.Callvirt, m_Scene_Add);

            // We could have dup'd the pre-existing Level object, but this produces a cleaner decomp (replacing the Level objects we cannibalized)
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Callvirt, m_LevelLoader_get_Level);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Callvirt, m_LevelLoader_get_Level);
        }

        public static void PatchTransitionRoutine(MethodDefinition method, CustomAttribute attrib) {
            MethodDefinition m_GCCollect = method.DeclaringType.FindMethod("System.Void _GCCollect()");

            // The level transition routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                break;
            }

            ILCursor cursor = new ILCursor(new ILContext(method));
            cursor.GotoNext(instr => instr.MatchCall("System.GC", "Collect"));
            // Replace the method call.
            cursor.Next.Operand = m_GCCollect;

        }

        public static void PatchErrorLogWrite(ILContext context, CustomAttribute attrib) {
            if (Everest == null)
                Everest = MonoModRule.Modder.FindType("Celeste.Mod.Everest").Resolve();

            if (m_Everest_get_VersionCelesteString == null)
                m_Everest_get_VersionCelesteString = Everest.FindMethod("System.String get_VersionCelesteString()");

            /* We expect something similar enough to the following:
            call	 class Monocle.Engine Monocle.Engine::get_Instance() // We're here
            ldfld    class [mscorlib] System.Version Monocle.Engine::Version 
            callvirt instance string[mscorlib] System.Object::ToString() 

            Note that MonoMod requires the full type names (System.String instead of string)
            */

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchCall("Monocle.Engine", "get_Instance"),
                instr => instr.MatchLdfld("Monocle.Engine", "Version"),
                instr => instr.MatchCallvirt("System.Object", "ToString"));

            // Remove all that and replace with our own string.
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Call, m_Everest_get_VersionCelesteString);
        }

        public static void PatchHeartGemCollectRoutine(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));

            FieldDefinition f_this = null;
            FieldDefinition f_completeArea = null;

            MethodDefinition m_IsCompleteArea = method.DeclaringType.FindMethod("System.Boolean IsCompleteArea(System.Boolean)");

            // The gem collection routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<CollectRoutine>d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                f_this = method.DeclaringType.FindField("<>4__this");
                f_completeArea = method.DeclaringType.Fields.FirstOrDefault(f => f.Name.StartsWith("<completeArea>5__"));
                break;
            }

            new ILContext(method).Invoke(ctx => {
                ILCursor cursor = new ILCursor(ctx);

                cursor.GotoNext(instr => instr.MatchLdfld("Celeste.HeartGem", "IsFake"));
                // Push "this" onto stack, and retrieve the actual HeartGem `this`
                cursor.Emit(OpCodes.Ldarg_0);
                cursor.Emit(OpCodes.Ldfld, f_this);

                // Pre-process the bool on stack before
                // stfld    bool Celeste.HeartGem/'<CollectRoutine>d__29'::'<completeArea>5__4'
                // No need to check for the full name when the field name itself is compiler-generated.
                // Using AfterLabel to redirect break instructions to the right place.
                cursor.GotoNext(MoveType.AfterLabel, instr => instr.MatchStfld(f_completeArea.DeclaringType.FullName, f_completeArea.Name));
                // Process.
                cursor.Emit(OpCodes.Call, m_IsCompleteArea);
            });
        }

        public static void PatchBadelineChaseRoutine(MethodDefinition method, CustomAttribute attrib) {
            FieldDefinition f_this = null;

            MethodDefinition m_CanChangeMusic = method.DeclaringType.FindMethod("System.Boolean Celeste.BadelineOldsite::CanChangeMusic(System.Boolean)");
            MethodDefinition m_IsChaseEnd = method.DeclaringType.FindMethod("System.Boolean Celeste.BadelineOldsite::IsChaseEnd(System.Boolean)");

            // The routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                f_this = method.DeclaringType.FindField("<>4__this");
                break;
            }

            ILCursor cursor = new ILCursor(new ILContext(method));

            // Add this.CanChangeMusic()
            cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldarg_0,
                instr => instr.MatchLdfld(out FieldReference f) && f.Name == "level");
            // Push this and grab this from this.
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_this);

            cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld("Celeste.AreaKey", "Mode"));
            if (cursor.Next.OpCode == OpCodes.Brtrue_S) {
                // Insert `== 0`
                cursor.Emit(OpCodes.Ldc_I4_0);
                cursor.Emit(OpCodes.Ceq);
                // Replace brtrue with brfalse
                cursor.Next.OpCode = OpCodes.Brfalse_S;
            } else {
                // SteamFNA version, `== 0` is already there
                cursor.GotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ceq);
            }
            // Process.
            cursor.Emit(OpCodes.Call, m_CanChangeMusic);

            // Add this.IsChaseEnd()
            cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldarg_0,
                    instr => instr.MatchLdfld(out FieldReference f) && f.Name == "level",
                    instr => true, instr => true, instr => instr.MatchLdstr("2"));
            // Push this and grab this from this.
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_this);

            cursor.GotoNext(MoveType.After, instr => instr.MatchLdstr("2"),
                instr => instr.MatchCall<string>("op_Equality"));
            // Process.
            cursor.Emit(OpCodes.Call, m_IsChaseEnd);
        }

        public static void PatchBadelineBossOnPlayer(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_CanChangeMusic = context.Method.DeclaringType.FindMethod("System.Boolean Celeste.FinalBoss::CanChangeMusic(System.Boolean)");

            ILCursor cursor = new ILCursor(context);

            cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld("Celeste.AreaKey", "Mode"));
            if (cursor.Next.OpCode == OpCodes.Brtrue) {
                // Insert `== 0`
                cursor.Emit(OpCodes.Ldc_I4_0);
                cursor.Emit(OpCodes.Ceq);
                // Replace brtrue with brfalse
                cursor.Next.OpCode = OpCodes.Brfalse_S;
            } else {
                // SteamFNA version, `== 0` is already there
                cursor.GotoNext(MoveType.After, instr => instr.OpCode == OpCodes.Ceq);
            }
            // Process.
            cursor.Emit(OpCodes.Call, m_CanChangeMusic);

            // Go back to the start of this "line" and add `this` to be used by CanChangeMusic()
            cursor.GotoPrev(instr => instr.OpCode == OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldarg_0);

        }

        public static void PatchCloudAdded(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_IsSmall = context.Method.DeclaringType.FindMethod("System.Boolean Celeste.Cloud::IsSmall(System.Boolean)");

            ILCursor cursor = new ILCursor(context);

            cursor.GotoNext(instr => instr.MatchCall(out MethodReference m) && m.Name == "SceneAs");
            // Push `this`, to be used later by `IsSmall`
            cursor.Emit(OpCodes.Ldarg_0);

            /* We expect something similar enough to the following:
            ldfld    Celeste.AreaMode Celeste.AreaKey::Mode
            ldc.i4.0
            cgt.un    // We're here
            */
            bool isSteamFNA = cursor.TryGotoNext(MoveType.After, instr => instr.MatchLdfld("Celeste.AreaKey", "Mode"),
                instr => instr.OpCode == OpCodes.Ldc_I4_0,
                instr => instr.MatchCgtUn());

            // Alternatively:
            /* We expect something similar enough to the following:
            ldfld    Celeste.AreaMode Celeste.AreaKey::Mode
            brfalse.s    // We're here
            */
            if (!isSteamFNA)
                // We want to be BEFORE !=
                cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld("Celeste.AreaKey", "Mode") &&
                    instr.Next.OpCode == OpCodes.Brfalse_S);

            // Process.
            cursor.Emit(OpCodes.Call, m_IsSmall);
        }

        public static void PatchRainFGRender(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_GetColor = context.Method.DeclaringType.FindMethod("Microsoft.Xna.Framework.Color Celeste.RainFG::_GetColor(System.String)");

            ILCursor cursor = new ILCursor(context);
            // AfterLabel to redirect break instructions
            cursor.GotoNext(MoveType.AfterLabel, instr => instr.MatchLdstr("161933"));
            // Push `this`.
            cursor.Emit(OpCodes.Ldarg_0);
            // Replace the `Calc.HexToColor` method call after ldstr.
            cursor.Next.Next.Operand = m_GetColor;
        }

        public static void PatchDialogLoader(MethodDefinition method, CustomAttribute attrib) {
            // Our actual target method is the orig_ method.
            method = method.DeclaringType.FindMethod(method.GetID(name: method.GetOriginalName()));

            MethodDefinition m_GetFiles = method.DeclaringType.FindMethod("System.String[] _GetFiles(System.String,System.String,System.IO.SearchOption)");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.MatchCall("System.IO.Directory", "GetFiles")) {
                    instr.Operand = m_GetFiles;
                }
            }
        }

        public static void PatchLoadLanguage(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_GetLanguageText = context.Method.DeclaringType.FindMethod("System.Collections.Generic.IEnumerable`1<System.String> _GetLanguageText(System.String,System.Text.Encoding)");
            MethodDefinition m_NewLanguage = context.Method.DeclaringType.FindMethod("Celeste.Language _NewLanguage()");
            MethodDefinition m_SetItem = context.Method.DeclaringType.FindMethod("System.Void _SetItem(System.Collections.Generic.Dictionary`2<System.String,System.String>,System.String,System.String,Celeste.Language)");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchCall("System.IO.File", "ReadLines"));
            cursor.Next.Operand = m_GetLanguageText;

            cursor.GotoNext(instr => instr.MatchNewobj("Celeste.Language"));
            cursor.Next.OpCode = OpCodes.Call;
            cursor.Next.Operand = m_NewLanguage;

            // Start again from the top
            cursor.Goto(cursor.Instrs[0]);
            int matches = 0;
            while (cursor.TryGotoNext(instr => instr.MatchCallvirt("System.Collections.Generic.Dictionary`2<System.String,System.String>", "set_Item"))) {
                matches++;
                // Push the language object. Should always be stored in the first local var.
                cursor.Emit(OpCodes.Ldloc_0);
                // Replace the method call.
                cursor.Next.OpCode = OpCodes.Call;
                cursor.Next.Operand = m_SetItem;
            }
            if (matches != 3)
                throw new Exception("Incorrect number of matches for language.Dialog.set_Item");
            
        }

        public static void PatchInitMMSharedData(MethodDefinition method, CustomAttribute attrib) {
            MethodDefinition m_Set = method.DeclaringType.FindMethod("System.Void SetMMSharedData(System.String,System.Boolean)");

            method.Body.Instructions.Clear();
            ILProcessor il = method.Body.GetILProcessor();

            foreach (KeyValuePair<string, object> kvp in MonoModRule.Modder.SharedData) {
                if (!(kvp.Value is bool))
                    return;
                il.Emit(OpCodes.Ldstr, kvp.Key);
                il.Emit((bool) kvp.Value ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                il.Emit(OpCodes.Call, m_Set);
            }

            il.Emit(OpCodes.Ret);
        }


        public static void RegisterLevelExitRoutine(MethodDefinition method, CustomAttribute attrib) {
            // Register it. Don't patch it directly as we require an explicit patching order.
            LevelExitRoutines.Add(method);
        }

        public static void PatchLevelExitRoutine(MethodDefinition method) {
            FieldDefinition f_completeMeta = method.DeclaringType.FindField("completeMeta");
            FieldDefinition f_this = null;

            // The level exit routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                f_this = method.DeclaringType.FindField("<>4__this");
                break;
            }

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];
                MethodReference calling = instr.Operand as MethodReference;
                string callingID = calling?.GetID();

                // The original AreaComplete .ctor has been modified to contain an extra parameter.
                // For safety, check against both signatures.
                if (instr.OpCode != OpCodes.Newobj || (
                    callingID != "System.Void Celeste.AreaComplete::.ctor(Celeste.Session,System.Xml.XmlElement,Monocle.Atlas,Celeste.HiresSnow)" &&
                    callingID != "System.Void Celeste.AreaComplete::.ctor(Celeste.Session,System.Xml.XmlElement,Monocle.Atlas,Celeste.HiresSnow,Celeste.Mod.Meta.MapMetaCompleteScreen)"
                ))
                    continue;

                // For safety, replace the .ctor call if the new .ctor exists already.
                instr.Operand = calling.DeclaringType.Resolve().FindMethod("System.Void Celeste.AreaComplete::.ctor(Celeste.Session,System.Xml.XmlElement,Monocle.Atlas,Celeste.HiresSnow,Celeste.Mod.Meta.MapMetaCompleteScreen)") ?? instr.Operand;

                instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));

                if (f_this != null) {
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                }

                instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_completeMeta));

            }

        }

        public static void RegisterAreaCompleteCtor(MethodDefinition method, CustomAttribute attrib) {
            // Register it. Don't patch it directly as we require an explicit patching order.
            AreaCompleteCtors.Add(method);
        }

        public static void PatchAreaCompleteCtor(MethodDefinition method) {
            ParameterDefinition paramMeta = new ParameterDefinition("meta", ParameterAttributes.None, MonoModRule.Modder.FindType("Celeste.Mod.Meta.MapMetaCompleteScreen"));
            method.Parameters.Add(paramMeta);

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];
                MethodReference calling = instr.Operand as MethodReference;
                string callingID = calling?.GetID();

                // The matching CompleteRenderer .ctor has been added manually, thus manually relink to it.
                if (instr.OpCode != OpCodes.Newobj || (
                    callingID != "System.Void Celeste.CompleteRenderer::.ctor(System.Xml.XmlElement,Monocle.Atlas,System.Single,System.Action)"
                ))
                    continue;

                instr.Operand = calling.DeclaringType.Resolve().FindMethod("System.Void Celeste.CompleteRenderer::.ctor(System.Xml.XmlElement,Monocle.Atlas,System.Single,System.Action,Celeste.Mod.Meta.MapMetaCompleteScreen)");

                instrs.Insert(instri++, il.Create(OpCodes.Ldarg, paramMeta));
            }

            ILCursor cursor = new ILCursor(new ILContext(method));
            MethodDefinition m_GetCustomCompleteScreenTitle = method.DeclaringType.FindMethod("System.String GetCustomCompleteScreenTitle()");

            int textVariableIndex = 0;

            /*
             * // string text = Dialog.Clean("areacomplete_" + session.Area.Mode + (session.FullClear ? "_fullclear" : ""), null);
             * IL_005D: ldstr     "areacomplete_"
             * IL_0062: ldarg.1
             * IL_0063: ldflda    valuetype Celeste.AreaKey Celeste.Session::Area
             * ...
             * IL_008B: ldnull
             * IL_008C: call      string Celeste.Dialog::Clean(string, class Celeste.Language)
             * IL_0091: stloc.1
             *
             * // Vector2 origin = new Vector2(960f, 200f);
             * IL_0092: ldloca.s  V_2
             * IL_0094: ldc.r4    960
             * ...
             */

            // move the cursor to IL_0092 and find the variable index of "text"
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Celeste.Dialog", "Clean"),
                instr => instr.MatchStloc(out textVariableIndex) && il.Body.Variables[textVariableIndex].VariableType.FullName == "System.String");

            // mark for later use
            ILLabel target = cursor.MarkLabel();
            // go back to IL_005D
            cursor.GotoPrev(MoveType.Before, instr => instr.MatchLdstr("areacomplete_"));

            // equivalent to "text = this.GetCustomCompleteScreenTitle()"
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_GetCustomCompleteScreenTitle);
            cursor.Emit(OpCodes.Stloc_S, (byte) textVariableIndex);

            // wrap the original text assignment code in "if (text == null)", fallback to original if no custom title in meta.yaml
            cursor.Emit(OpCodes.Ldloc_S, (byte) textVariableIndex);
            cursor.Emit(OpCodes.Brtrue_S, target.Target);
        }


        public static void PatchGameLoaderIntroRoutine(MethodDefinition method, CustomAttribute attrib) {
            MethodDefinition m_GetNextScene = method.DeclaringType.FindMethod("Monocle.Scene _GetNextScene(Celeste.Overworld/StartMode,Celeste.HiresSnow)");

            // The routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                break;
            }

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.OpCode == OpCodes.Newobj && (instr.Operand as MethodReference)?.GetID() == "System.Void Celeste.OverworldLoader::.ctor(Celeste.Overworld/StartMode,Celeste.HiresSnow)") {
                    instr.OpCode = OpCodes.Call;
                    instr.Operand = m_GetNextScene;
                }
            }
        }

        public static void PatchSaveRoutine(MethodDefinition method, CustomAttribute attrib) {
            if (SaveData == null)
                SaveData = MonoModRule.Modder.FindType("Celeste.SaveData")?.Resolve();

            FieldDefinition f_Instance = SaveData.FindField("Instance");

            MethodDefinition m_AfterInitialize = SaveData.FindMethod("System.Void AfterInitialize()");

            // The routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                break;
            }

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.OpCode == OpCodes.Call && (instr.Operand as MethodReference)?.GetID() == "System.Byte[] Celeste.UserIO::Serialize<Celeste.SaveData>(T)") {
                    instri++;

                    instrs.Insert(instri++, il.Create(OpCodes.Ldsfld, f_Instance));

                    instrs.Insert(instri++, il.Create(OpCodes.Callvirt, m_AfterInitialize));
                }
            }
        }

        public static void PatchPlayerOrigUpdate(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_IsOverWater = context.Method.DeclaringType.FindMethod("System.Boolean _IsOverWater()");

            Mono.Collections.Generic.Collection<Instruction> instrs = context.Body.Instructions;
            ILProcessor il = context.Body.GetILProcessor();
            for (int instri = 1; instri < instrs.Count - 5; instri++) {
                // turn "if (Speed.Y < 0f && Speed.Y >= -60f)" into "if (Speed.Y < 0f && Speed.Y >= -60f && _IsOverWater())"
                if (instrs[instri].OpCode == OpCodes.Ldarg_0
                    && instrs[instri + 1].MatchLdflda("Celeste.Player", "Speed")
                    && instrs[instri + 2].MatchLdfld("Microsoft.Xna.Framework.Vector2", "Y")
                    && instrs[instri + 3].MatchLdcR4(-60f)) {

                    // XNA:
                    // 0: ldarg.0
                    // 1: ldflda Celeste.Player::Speed
                    // 2: ldfld Vector2::Y
                    // 3: ldc.r4 -60
                    // 4: blt.un [instruction after if]
                    if (instrs[instri + 4].OpCode == OpCodes.Blt_Un) {
                        // 5: ldarg.0
                        // 6: call Player::_IsOverWater
                        // 7: brfalse [instruction after if]
                        instrs.Insert(instri + 5, il.Create(OpCodes.Ldarg_0));
                        instrs.Insert(instri + 6, il.Create(OpCodes.Call, m_IsOverWater));
                        instrs.Insert(instri + 7, il.Create(OpCodes.Brfalse, instrs[instri + 4].Operand));
                    }

                    // FNA:
                    // -1: bge.un.s [instruction setting flag to false]
                    // 0: ldarg.0
                    // 1: ldflda Celeste.Player::Speed
                    // 2: ldfld Vector2::Y
                    // 3: ldc.r4 -60
                    // 4: clt.un
                    // 5: ldc.i4.0
                    // 6: ceq [final value for the flag. if this flag is true, we enter the if]
                    if (instrs[instri - 1].OpCode == OpCodes.Bge_Un_S
                        && instrs[instri + 4].OpCode == OpCodes.Clt_Un
                        && instrs[instri + 5].OpCode == OpCodes.Ldc_I4_0) {
                        // 4: blt.un [instruction setting flag to false]
                        // 5: ldarg.0
                        // 6: call Player::_IsOverWater
                        // 7: ldc.i4.1
                        // 8: ceq
                        instrs[instri + 4].OpCode = OpCodes.Blt_Un;
                        instrs[instri + 4].Operand = instrs[instri - 1].Operand;

                        instrs.Insert(instri + 5, il.Create(OpCodes.Ldarg_0));
                        instrs.Insert(instri + 6, il.Create(OpCodes.Call, m_IsOverWater));

                        instrs[instri + 7].OpCode = OpCodes.Ldc_I4_1;

                    }
                }
            }
        }

        public static void PatchChapterPanelSwapRoutine(MethodDefinition method, CustomAttribute attrib) {
            FieldDefinition f_this;

            MethodDefinition m_GetCheckpoints = method.DeclaringType.FindMethod("System.Collections.Generic.HashSet`1<System.String> _GetCheckpoints(Celeste.SaveData,Celeste.AreaKey)");

            // The gem collection routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<SwapRoutine>d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                f_this = method.DeclaringType.FindField("<>4__this");
                break;
            }

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 1; instri < instrs.Count - 5; instri++) {
                Instruction instr = instrs[instri];

                if (instr.OpCode == OpCodes.Callvirt && (instr.Operand as MethodReference)?.GetID() == "System.Collections.Generic.HashSet`1<System.String> Celeste.SaveData::GetCheckpoints(Celeste.AreaKey)") {
                    // Replace the method call.
                    instr.OpCode = OpCodes.Call;
                    instr.Operand = m_GetCheckpoints;
                    instri++;
                }
            }
        }

        public static void PatchStrawberryInterface(ICustomAttributeProvider provider, CustomAttribute attrib) {
            // MonoModRule.Modder.FindType("Celeste.Mod.IStrawberry");
            if (IStrawberry == null)
                IStrawberry = new InterfaceImplementation(MonoModRule.Modder.FindType("Celeste.Mod.IStrawberry"));

            ((TypeDefinition) provider).Interfaces.Add(IStrawberry);
        }

        public static void PatchInterface(MethodDefinition method, CustomAttribute attrib) {
            MethodAttributes flags = MethodAttributes.Virtual | MethodAttributes.Final | MethodAttributes.NewSlot;
            method.Attributes |= flags;
        }

        public static void PatchFileSelectSlotRender(ILContext context, CustomAttribute attrib) {
            TypeDefinition declaringType = context.Method.DeclaringType;
            FieldDefinition f_maxStrawberryCount = declaringType.FindField("maxStrawberryCount");
            FieldDefinition f_maxGoldenStrawberryCount = declaringType.FindField("maxGoldenStrawberryCount");
            FieldDefinition f_maxCassettes = declaringType.FindField("maxCassettes");
            FieldDefinition f_maxCrystalHeartsExcludingCSides = declaringType.FindField("maxCrystalHeartsExcludingCSides");
            FieldDefinition f_maxCrystalHearts = declaringType.FindField("maxCrystalHearts");
            FieldDefinition f_summitStamp = declaringType.FindField("summitStamp");
            FieldDefinition f_farewellStamp = declaringType.FindField("farewellStamp");
            FieldDefinition f_totalGoldenStrawberries = declaringType.FindField("totalGoldenStrawberries");
            FieldDefinition f_totalHeartGems = declaringType.FindField("totalHeartGems");
            FieldDefinition f_totalCassettes = declaringType.FindField("totalCassettes");

            ILCursor cursor = new ILCursor(context);
            // SaveData.TotalStrawberries replaced by SaveData.TotalStrawberries_Safe with MonoModLinkFrom
            // Replace hardcoded ARB value with a field reference
            cursor.GotoNext(MoveType.After, instr => instr.MatchLdcI4(175));
            cursor.Prev.OpCode = OpCodes.Ldarg_0;
            cursor.Emit(OpCodes.Ldfld, f_maxStrawberryCount);


            // SaveData.Areas replaced by SaveData.Areas_Safe with MonoModLinkFrom
            // We want to replace `this.SaveData.Areas_Safe[7].Modes[0].Completed`
            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchCallvirt("Celeste.SaveData", "get_Areas_Safe"),
                instr => instr.OpCode == OpCodes.Ldc_I4_7);
            // Remove everything but the preceeding `this`
            cursor.RemoveRange(8);
            // Replace with `this.summitStamp`
            cursor.Emit(OpCodes.Ldfld, f_summitStamp);


            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchCallvirt("Celeste.SaveData", "get_TotalCassettes"));
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Ldfld, f_totalCassettes);
            // Replace hardcoded Cassettes value with a field reference
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_maxCassettes);


            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchCallvirt("Celeste.SaveData", "get_TotalHeartGems"));
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Ldfld, f_totalHeartGems);
            // Replace hardcoded HeartGems value with a field reference
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_maxCrystalHeartsExcludingCSides);


            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchLdfld("Celeste.SaveData", "TotalGoldenStrawberries"));
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Ldfld, f_totalGoldenStrawberries);
            // Replace hardcoded GoldenStrawberries value with a field reference
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_maxGoldenStrawberryCount);


            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchCallvirt("Celeste.SaveData", "get_TotalHeartGems"));
            cursor.RemoveRange(3);
            cursor.Emit(OpCodes.Ldfld, f_totalHeartGems);
            // Replace hardcoded HeartGems value with a field reference
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_maxCrystalHearts);

            // SaveData.Areas replaced by SaveData.Areas_Safe with MonoModLinkFrom
            // We want to replace `this.SaveData.Areas_Safe[10].Modes[0].Completed`
            cursor.GotoNext(instr => instr.MatchLdfld(declaringType.FullName, "SaveData"),
                instr => instr.MatchCallvirt("Celeste.SaveData", "get_Areas_Safe"),
                instr => instr.MatchLdcI4(10));
            // Remove everything but the preceeding `this`
            cursor.RemoveRange(8);
            // Replace with `this.farewellStamp`
            cursor.Emit(OpCodes.Ldfld, f_farewellStamp);

        }

        public static void PatchPathfinderRender(ILContext context, CustomAttribute attrib) {
            FieldDefinition f_map = context.Method.DeclaringType.FindField("map");

            ILCursor cursor = new ILCursor(context);
            for (int i = 0; i < 2; i++) {
                cursor.GotoNext(MoveType.After, instr => instr.MatchLdcI4(200));
                cursor.Prev.OpCode = OpCodes.Ldarg_0;
                cursor.Emit(OpCodes.Ldfld, f_map);
                // if `i == 0` we are accessing the first dimension, `1` we are accessing the second
                cursor.Emit(i == 0 ? OpCodes.Ldc_I4_1 : OpCodes.Ldc_I4_0);
                cursor.Emit(OpCodes.Callvirt, typeof(Array).GetMethod("GetLength"));
            }

        }

        public static void PatchTotalHeartGemChecks(MethodDefinition method, CustomAttribute attrib) {
            MethodDefinition m_getTotalHeartGemsInVanilla = method.Module.GetType("Celeste.SaveData").FindMethod("System.Int32 get_TotalHeartGemsInVanilla()");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.MatchCallvirt(out MethodReference m) && m.Name == "get_TotalHeartGems") {
                    // replace the call to the TotalHeartGems property with a call to TotalHeartGemsInVanilla.
                    instr.Operand = m_getTotalHeartGemsInVanilla;
                }
            }
        }
        public static void PatchTotalHeartGemChecksInRoutine(MethodDefinition method, CustomAttribute attrib) {
            // Routines are stored in compiler-generated methods.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                break;
            }

            PatchTotalHeartGemChecks(method, attrib);
        }

        public static void PatchOuiJournalStatsHeartGemCheck(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_getUnlockedModes = context.Method.Module.GetType("Celeste.SaveData").FindMethod("System.Int32 get_UnlockedModes()");

            ILCursor cursor = new ILCursor(context);

            /* 
            We want to replace `SaveData.Instance.TotalHeartGems >= 16` with `SaveData.Instance.UnlockedModes >= 3`.
            This way, we only display the golden berry stat when golden berries are actually unlocked in the level set we are in.
            (UnlockedModes returns 3 if and only if TotalHeartGems is more than 16 in the vanilla level set anyway.)
            */

            // Move between these two instructions
            cursor.GotoNext(MoveType.After, instr => instr.MatchCallvirt("Celeste.SaveData", "get_TotalHeartGems") &&
                instr.Next.MatchLdcI4(16));
            cursor.Prev.Operand = m_getUnlockedModes;
            cursor.Next.OpCode = OpCodes.Ldc_I4_3;
        }

        public static void MakeMethodPublic(MethodDefinition method, CustomAttribute attrib) {
            method.SetPublic(true);
        }

        public static void PatchSpinnerCreateSprites(ILContext context, CustomAttribute attrib) {
            FieldDefinition f_ID = context.Method.DeclaringType.FindField("ID");

            ILCursor cursor = new ILCursor(context);

            // instead of comparing the X positions for spinners, compare their IDs.
            // this way, we are sure spinner 1 will connect to spinner 2, but spinner 2 won't connect to spinner 1.
            cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldloc_S,
                instr => instr.OpCode == OpCodes.Ldarg_0,
                instr => instr.OpCode == OpCodes.Beq_S);
            // Move after `ldloc_s`
            cursor.Goto(cursor.Next, MoveType.After);
            cursor.Emit(OpCodes.Ldfld, f_ID);
            // Move after `ldarg_0`
            cursor.Goto(cursor.Next, MoveType.After);
            cursor.Emit(OpCodes.Ldfld, f_ID);
            // Replace `beq_s`(!=) with `ble_s`(>)
            cursor.Next.OpCode = OpCodes.Ble_S;

            // the other.X >= this.X check is made redundant by the patch above. Remove it.
            cursor.GotoNext(instr => instr.OpCode == OpCodes.Ldloc_S,
                instr => instr.MatchCallvirt("Monocle.Entity", "get_X"));
            cursor.RemoveRange(5);

            // replace `(item.Position - Position).Length() < 24f` with `.LengthSquared() < 576f`.
            // this is equivalent, except it skips a square root calculation, which helps with performance.
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Microsoft.Xna.Framework.Vector2", "Length"));
            ((MethodReference) cursor.Prev.Operand).Name = "LengthSquared";
            cursor.Next.Operand = 576f;
        }

        public static void PatchOuiFileSelectSubmenuChecks(MethodDefinition method, CustomAttribute attrib) {
            TypeDefinition t_ISubmenu = method.Module.GetType("Celeste.OuiFileSelectSlot/ISubmenu");


            // The routine is stored in a compiler-generated method.
            string methodName = method.Name;
            if (methodName.StartsWith("orig_")) {
                methodName = methodName.Substring(5);
            }
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + methodName + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                break;
            }

            ILProcessor il = method.Body.GetILProcessor();
            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count - 4; instri++) {
                if (instrs[instri].OpCode == OpCodes.Brtrue_S
                    && instrs[instri + 1].OpCode == OpCodes.Ldarg_0
                    && instrs[instri + 2].OpCode == OpCodes.Ldfld
                    && instrs[instri + 3].MatchIsinst("Celeste.OuiAssistMode")) {

                    // gather some info
                    FieldReference field = (FieldReference) instrs[instri + 2].Operand;
                    Instruction branchTarget = (Instruction) instrs[instri].Operand;

                    // then inject another similar check for ISubmenu
                    instri++;
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, field));
                    instrs.Insert(instri++, il.Create(OpCodes.Isinst, t_ISubmenu));
                    instrs.Insert(instri++, il.Create(OpCodes.Brtrue_S, branchTarget));

                } else if (instrs[instri].OpCode == OpCodes.Ldarg_0
                    && instrs[instri + 1].OpCode == OpCodes.Ldfld
                    && instrs[instri + 2].MatchIsinst("Celeste.OuiAssistMode")
                    && instrs[instri + 3].OpCode == OpCodes.Brfalse_S) {

                    // gather some info
                    FieldReference field = (FieldReference) instrs[instri + 1].Operand;
                    Instruction branchTarget = instrs[instri + 4];

                    // then inject another similar check for ISubmenu
                    instri++;
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, field));
                    instrs.Insert(instri++, il.Create(OpCodes.Isinst, t_ISubmenu));
                    instrs.Insert(instri++, il.Create(OpCodes.Brtrue_S, branchTarget));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                }
            }
        }

        public static void PatchFontsPrepare(MethodDefinition method, CustomAttribute attrib) {
            MethodDefinition m_GetFiles = method.DeclaringType.FindMethod("System.String[] _GetFiles(System.String,System.String,System.IO.SearchOption)");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 0; instri < instrs.Count; instri++) {
                if (instrs[instri].MatchCall("System.IO.Directory", "GetFiles")) {
                    instrs[instri].Operand = m_GetFiles;
                }
            }
        }

        public static void MakeEntryPoint(MethodDefinition method, CustomAttribute attrib) {
            MonoModRule.Modder.Module.EntryPoint = method;
        }

        public static void PatchCelesteMain(ILContext context, CustomAttribute attrib) {
            ILCursor cursor = new ILCursor(context);
            // TryGotoNext used because SDL_GetPlatform does not exist on XNA
            if (cursor.TryGotoNext(instr => instr.MatchCall("SDL2.SDL", "SDL_GetPlatform"))) {
                cursor.Next.OpCode = OpCodes.Ldstr;
                cursor.Next.Operand = "Windows";
            }
        }

        public static void RemoveCommandAttributeFromVanillaLoadMethod(MethodDefinition method, CustomAttribute attrib) {
            // find the vanilla method: CmdLoadIDorSID(string, string) => CmdLoad(int, string)
            string vanillaMethodName = method.Name.Replace("IDorSID", "");
            Mono.Collections.Generic.Collection<CustomAttribute> attributes = method.DeclaringType.FindMethod($"System.Void {vanillaMethodName}(System.Int32,System.String)").CustomAttributes;
            for (int i = 0; i < attributes.Count; i++) {
                // remove all Command attributes.
                if (attributes[i]?.AttributeType.FullName == "Monocle.Command") {
                    attributes.RemoveAt(i);
                    i--;
                }
            }
        }

        public static void PatchFakeHeartColor(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_getCustomColor = context.Method.DeclaringType.FindMethod("Celeste.AreaMode Celeste.FakeHeart::_GetCustomColor(Celeste.AreaMode)");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchLdsfld("Monocle.Calc", "Random"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Monocle.Calc", "Choose"));
            cursor.Emit(OpCodes.Call, m_getCustomColor);
        }

        public static void PatchOuiFileNamingRendering(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_shouldDisplaySwitchAlphabetPrompt = context.Method.DeclaringType.FindMethod("System.Boolean _shouldDisplaySwitchAlphabetPrompt()");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchCallvirt("Celeste.OuiFileNaming", "get_Japanese"));
            cursor.Next.OpCode = OpCodes.Call;
            cursor.Next.Operand = m_shouldDisplaySwitchAlphabetPrompt;
        }

        public static void PatchRumbleTriggerAwake(MethodDefinition method, CustomAttribute attrig) {
            MethodDefinition m_entity_get_Y = MonoModRule.Modder.FindType("Monocle.Entity").Resolve().FindMethod("get_Y");

            FieldDefinition f_constrainHeight = method.DeclaringType.FindField("constrainHeight");
            FieldDefinition f_top = method.DeclaringType.FindField("top");
            FieldDefinition f_bottom = method.DeclaringType.FindField("bottom");

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];
                /*
                        ldloc.*
                        callvirt	instance float32 Monocle.Entity::get_X()
                        ldarg.0
                        ldfld	float32 Celeste.RumbleTrigger::left
                        blt.un.s	ldloca.s
                        ldloc.*
                        callvirt	instance float32 Monocle.Entity::get_X()
                        ldarg.0
                        ldfld	float32 Celeste.RumbleTrigger::right // We are here
                        bgt.un.s	ldloca.s
                     */
                if (instr.MatchLdfld("Celeste.RumbleTrigger", "right")) {
                    Instruction noYConstraintTarget = instrs[instri - 8];

                    // Copy relevant instructions and modify as needed
                    Instruction[] instrCopy = new Instruction[10];
                    for (int i = 0; i < 10; i++) {
                        instrCopy[i] = il.Create(instrs[instri + i - 8].OpCode, instrs[instri + i - 8].Operand);
                        if (instrCopy[i].OpCode == OpCodes.Callvirt)
                            instrCopy[i].Operand = m_entity_get_Y;
                        if (instrCopy[i].MatchLdfld("Celeste.RumbleTrigger", "left"))
                            instrCopy[i].Operand = f_top;
                        if (instrCopy[i].MatchLdfld("Celeste.RumbleTrigger", "right"))
                            instrCopy[i].Operand = f_bottom;
                        if (instrCopy[i].OpCode == OpCodes.Cgt_Un) {
                            // we are in Steam FNA, and want to replace this with a blg.un.s with the same target as 5 instructions before
                            instrCopy[i].OpCode = OpCodes.Bgt_Un_S;
                            instrCopy[i].Operand = instrCopy[i - 5].Operand;
                        }
                    }

                    instri -= 8;
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_constrainHeight));
                    instrs.Insert(instri++, il.Create(OpCodes.Brfalse_S, noYConstraintTarget));

                    // Insert copied instructions
                    instrs.InsertRange(instri, instrCopy);
                    instri += instrCopy.Length;

                    instri += 8;
                }
            }
        }

        public static void PatchEventTriggerOnEnter(MethodDefinition method, CustomAttribute attrib) {
            // We also need to do special work in the cctor.
            MethodDefinition m_cctor = method.DeclaringType.FindMethod(".cctor");

            MethodDefinition m_TriggerCustomEvent = method.DeclaringType.FindMethod("System.Boolean TriggerCustomEvent(Celeste.EventTrigger,Celeste.Player,System.String)");

            FieldDefinition f_Event = method.DeclaringType.FindField("Event");

            FieldDefinition f_LoadStrings = method.DeclaringType.FindField("_LoadStrings");

            Mono.Collections.Generic.Collection<Instruction> cctor_instrs = m_cctor.Body.Instructions;
            ILProcessor cctor_il = m_cctor.Body.GetILProcessor();

            // Remove cctor ret for simplicity. Re-add later.
            cctor_instrs.RemoveAt(cctor_instrs.Count - 1);

            TypeDefinition td_LoadStrings = f_LoadStrings.FieldType.Resolve();
            MethodReference m_LoadStrings_Add = MonoModRule.Modder.Module.ImportReference(td_LoadStrings.FindMethod("Add"));
            m_LoadStrings_Add.DeclaringType = f_LoadStrings.FieldType;
            MethodReference m_LoadStrings_ctor = MonoModRule.Modder.Module.ImportReference(td_LoadStrings.FindMethod("System.Void .ctor()"));
            m_LoadStrings_ctor.DeclaringType = f_LoadStrings.FieldType;
            cctor_il.Emit(OpCodes.Newobj, m_LoadStrings_ctor);

            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            ILProcessor il = method.Body.GetILProcessor();
            for (int instri = 0; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                /* We expect something similar enough to the following:
                ldfld     string Celeste.EventTrigger::Event // We're here
                stloc*
                ldloc*
                call      uint32 '<PrivateImplementationDetails>'::ComputeStringHash(string)

                Note that MonoMod requires the full type names (System.UInt32 instead of uint32) and skips escaping 's
                */

                if (instri > 0 &&
                        instri < instrs.Count - 4 &&
                        instr.MatchLdfld("Celeste.EventTrigger", "Event") &&
                        instrs[instri + 1].MatchStloc(out int _) &&
                        instrs[instri + 2].MatchLdloc(out int _) &&
                        instrs[instri + 3].MatchCall("<PrivateImplementationDetails>", "ComputeStringHash")
                    ) {
                    // Insert a call to our own event handler here.
                    // If it returns true, return.

                    // Load "this" onto stack
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));

                    //Load Player parameter onto stack
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_1));

                    //Load Event field onto stack again
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_Event));

                    // Call our static custom event handler.
                    instrs.Insert(instri++, il.Create(OpCodes.Call, m_TriggerCustomEvent));

                    // If we returned false, branch to ldfld. We still have the event ID on stack.
                    // This basically translates to if (result) { pop; ldstr ""; }; ldfld ...
                    instrs.Insert(instri, il.Create(OpCodes.Brfalse_S, instrs[instri]));
                    instri++;
                    // Otherwise, pop the event and return to skip any original event handler.
                    instrs.Insert(instri++, il.Create(OpCodes.Pop));
                    instrs.Insert(instri++, il.Create(OpCodes.Ret));
                }

                if (instr.OpCode == OpCodes.Ldstr) {
                    cctor_il.Emit(OpCodes.Dup);
                    cctor_il.Emit(OpCodes.Ldstr, instr.Operand);
                    cctor_il.Emit(OpCodes.Callvirt, m_LoadStrings_Add);
                    cctor_il.Emit(OpCodes.Pop); // HashSet.Add returns a bool.
                }

            }

            cctor_il.Emit(OpCodes.Stsfld, f_LoadStrings);
            cctor_il.Emit(OpCodes.Ret);
        }

        public static void PatchWaterUpdate(ILContext context, CustomAttribute attrib) {
            MethodReference m_WaterInteraction_get_Bounds = MonoModRule.Modder.Module.GetType("Celeste.WaterInteraction").FindProperty("Bounds").GetMethod;
            TypeReference t_Rectangle = m_WaterInteraction_get_Bounds.ReturnType;
            MethodReference m_Rectangle_get_Center = MonoModRule.Modder.Module.ImportReference(t_Rectangle.Resolve().FindProperty("Center").GetMethod);
            TypeReference t_Point = m_Rectangle_get_Center.ReturnType;
            FieldReference f_Point_Y = MonoModRule.Modder.Module.ImportReference(t_Point.Resolve().FindField("Y"));

            MethodReference m_Component_get_Entity = MonoModRule.Modder.Module.GetType("Monocle.Component").FindMethod("Monocle.Entity get_Entity()");
            MethodReference m_Entity_CollideRect = MonoModRule.Modder.Module.GetType("Monocle.Entity").FindMethod($"System.Boolean CollideRect({t_Rectangle.FullName})");

            MethodReference m_Point_ToVector2 = MonoModRule.Modder.Module.GetType("Celeste.Mod.Extensions").FindMethod($"Microsoft.Xna.Framework.Vector2 Celeste.Mod.Extensions::ToVector2(Microsoft.Xna.Framework.Point)");

            VariableDefinition v_Bounds = new VariableDefinition(t_Rectangle);
            context.Body.Variables.Add(v_Bounds);

            // Steam FNA is weird, and stores the entity in variable 5 instead of 3.
            bool isSteamFNA = context.Body.Variables[5].VariableType.FullName == "Monocle.Entity";

            ILCursor cursor = new ILCursor(context);
            // Load the WaterInteraction Bounds into a local variable
            cursor.GotoNext(MoveType.After, instr => instr.MatchCallvirt("Monocle.Component", "get_Entity"));
            cursor.Prev.Operand = m_WaterInteraction_get_Bounds;
            cursor.Next.OpCode = OpCodes.Stloc_S;
            cursor.Next.Operand = v_Bounds;

            // Replace the collision check (I think this technically loses precision but nobody's complained yet)
            cursor.GotoNext(instr => instr.MatchCall("Monocle.Entity", "CollideCheck"));
            cursor.Next.Operand = m_Entity_CollideRect;

            // Replace the Rectangle creation, and any values used for it, with our Bounds value.
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Monocle.Entity", "get_Scene"));
            while (!cursor.Next.MatchNewobj("Microsoft.Xna.Framework.Rectangle")) {
                cursor.Remove();
            }
            cursor.Next.OpCode = OpCodes.Ldloc_S;
            cursor.Next.Operand = v_Bounds;

            // Start again from the top and retrieve the Bounds instead of the entity (but only up to a certain point)
            cursor.Goto(0);
            for (int i = 0;i<10;i++) {
                cursor.GotoNext(MoveType.After, instr => (!isSteamFNA && instr.OpCode == OpCodes.Ldloc_3) || isSteamFNA && instr.MatchLdloc(5));
                cursor.Prev.OpCode = OpCodes.Ldloca_S;
                cursor.Prev.Operand = v_Bounds;

                // Modify any method calls/field accesses to the Bounds
                if (cursor.Next.MatchCallvirt("Monocle.Entity", "get_Center")) {
                    cursor.Remove();
                    cursor.Emit(OpCodes.Call, m_Rectangle_get_Center);
                    if (cursor.Next.OpCode == OpCodes.Ldfld) {
                        cursor.Remove();
                        cursor.Emit(OpCodes.Ldfld, f_Point_Y);
                        cursor.Emit(OpCodes.Conv_R4);
                    } else
                        cursor.Emit(OpCodes.Call, m_Point_ToVector2);
                } else {
                    cursor.Prev.OpCode = OpCodes.Ldloc_S;
                    cursor.Prev.Operand = v_Bounds;
                }
            }

            // We have reached the end of the code to be patched, we can finally load the WaterInteraction's Entity and continue as normal
            cursor.GotoNext(instr => instr.Next.MatchIsinst("Celeste.Player"));
            if (isSteamFNA) {
                cursor.Emit(OpCodes.Ldloc_S, (byte) 4);
                cursor.Emit(OpCodes.Callvirt, m_Component_get_Entity);
                cursor.Emit(OpCodes.Stloc_S, (byte) 5);
            } else {
                cursor.Emit(OpCodes.Ldloc_2);
                cursor.Emit(OpCodes.Callvirt, m_Component_get_Entity);
                cursor.Emit(OpCodes.Stloc_3);
            }
        }

        public static void PatchFakeHeartDialog(MethodDefinition method, CustomAttribute attrib) {
            FieldReference f_fakeHeartDialog = method.DeclaringType.FindField("fakeHeartDialog");
            FieldReference f_keepGoingDialog = method.DeclaringType.FindField("keepGoingDialog");

            FieldDefinition f_this = null;

            // The routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()") ?? method;
                f_this = method.DeclaringType.FindField("<>4__this");
                break;
            }

            ILCursor cursor = new ILCursor(new ILContext(method));

            cursor.GotoNext(instr => instr.MatchLdstr("CH9_FAKE_HEART"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_this);
            cursor.Next.OpCode = OpCodes.Ldfld;
            cursor.Next.Operand = f_fakeHeartDialog;

            cursor.GotoNext(instr => instr.MatchLdstr("CH9_KEEP_GOING"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_this);
            cursor.Next.OpCode = OpCodes.Ldfld;
            cursor.Next.Operand = f_keepGoingDialog;
        }

        public static void PatchTextMenuOptionColor(ILContext context, CustomAttribute attrib) {
            FieldReference f_UnselectedColor = context.Method.DeclaringType.FindField("UnselectedColor");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchCall("Microsoft.Xna.Framework.Color", "get_White"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Next.OpCode = OpCodes.Ldfld;
            cursor.Next.Operand = f_UnselectedColor;
        }

        public static void PatchIntroCrusherSequence(MethodDefinition method, CustomAttribute attrib) {
            FieldReference f_triggered = method.DeclaringType.FindField("triggered");
            FieldReference f_manualTrigger = method.DeclaringType.FindField("manualTrigger");
            FieldReference f_delay = method.DeclaringType.FindField("delay");
            FieldReference f_speed = method.DeclaringType.FindField("speed");

            FieldReference f_this = null;

            // The gem collection routine is stored in a compiler-generated method.
            foreach (TypeDefinition nest in method.DeclaringType.NestedTypes) {
                if (!nest.Name.StartsWith("<" + method.Name + ">d__"))
                    continue;
                method = nest.FindMethod("System.Boolean MoveNext()");
                f_this = method.DeclaringType.FindField("<>4__this");
                break;
            }

            // Steam FNA is weird, and all the local variables are messed up.
            bool isSteamFNA = method.Body.Variables[2].VariableType.FullName != "Celeste.Player";

            ILProcessor il = method.Body.GetILProcessor();
            Mono.Collections.Generic.Collection<Instruction> instrs = method.Body.Instructions;
            for (int instri = 1; instri < instrs.Count; instri++) {
                Instruction instr = instrs[instri];

                if (instr.OpCode == (isSteamFNA ? OpCodes.Ldloc_1 : OpCodes.Ldloc_2) &&
                    instrs[instri + 1].OpCode == OpCodes.Brfalse_S) {
                    // Get the instruction at the beginning of this while loop.
                    Instruction loopTarget = (Instruction) instrs[instri + 1].Operand;
                    // Get the instruction after the end of this while statement
                    Instruction breakTarget = instrs.Last(i => i.Operand == loopTarget).Next;

                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_triggered));
                    instrs.Insert(instri++, il.Create(OpCodes.Brtrue_S, breakTarget));

                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_manualTrigger));
                    instrs.Insert(instri++, il.Create(OpCodes.Brtrue_S, loopTarget));
                }

                if (instr.OpCode == OpCodes.Ldloc_3 && // The value stored in loc_3 is different between versions, but it still works the same.
                    instrs[instri + 1].OpCode == OpCodes.Brfalse_S) {
                    // Get the instruction at the beginning of this while loop.
                    Instruction loopTarget = (Instruction) instrs[instri + 1].Operand;
                    // Get the instruction after the end of this while statement
                    Instruction breakTarget = instrs.Last(i => i.Operand == loopTarget).Next;

                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_manualTrigger));
                    instrs.Insert(instri++, il.Create(OpCodes.Brtrue_S, loopTarget));
                }

                // Allow for custom activation delay
                if (instr.MatchLdcR4(1.2f)) {
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instr.OpCode = OpCodes.Ldfld;
                    instr.Operand = f_this;
                    instrs.Insert(++instri, il.Create(OpCodes.Ldfld, f_delay));
                }

                // If the delay is less than, or equal to zero, don't add a shaker.
                if (instr.OpCode == OpCodes.Ldarg_0 &&
                    instrs[instri + 1].MatchLdfld(f_this.DeclaringType.FullName, "<shaker>5__3") &&
                    instrs[instri + 2].MatchCallvirt("Monocle.Entity", "Add")) {
                    Instruction breakTarget = instrs[instri + 3];

                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_delay));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldc_R4, 0f));
                    instrs.Insert(instri++, il.Create(OpCodes.Ble, breakTarget));

                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                }

                // Allow for custom movement speed
                if (instr.MatchLdcR4(2f) &&
                    instrs[instri + 1].MatchCall("Monocle.Engine", "get_DeltaTime")) {
                    instrs.Insert(instri++, il.Create(OpCodes.Ldarg_0));
                    instrs.Insert(instri++, il.Create(OpCodes.Ldfld, f_this));
                    instr.OpCode = OpCodes.Ldfld;
                    instr.Operand = f_speed;
                }
            }
        }

        public static void PatchOuiChapterPanelRender(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_ModCardTexture = context.Method.DeclaringType.FindMethod("System.String Celeste.OuiChapterPanel::_ModCardTexture(System.String)");

            ILCursor cursor = new ILCursor(context);
            int matches = 0;
            while (cursor.TryGotoNext(MoveType.AfterLabel, instr => instr.MatchLdstr(out string str) && str.StartsWith("areaselect/card"))) {
                // Move to before the string is loaded, but before the labels, so we can redirect break targets to a new instruction.
                // Push this.
                cursor.Emit(OpCodes.Ldarg_0);
                // Move after ldstr
                cursor.Goto(cursor.Next, MoveType.After);
                // Insert method call to modify the string.
                cursor.Emit(OpCodes.Call, m_ModCardTexture);
                matches++;
            }
            if (matches != 4)
                throw new Exception("Incorrect number of matches for string starting with \"areaselect/card\".");
        }

        public static void PatchGoldenBlockStaticMovers(ILContext context, CustomAttribute attrib) {
            TypeDefinition t_Platform = MonoModRule.Modder.Module.GetType("Celeste.Platform");
            MethodDefinition m_Platform_DisableStaticMovers = t_Platform.FindMethod("System.Void DisableStaticMovers()");
            MethodDefinition m_Platform_DestroyStaticMovers = t_Platform.FindMethod("System.Void DestroyStaticMovers()");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Celeste.Solid", "Awake"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_Platform_DisableStaticMovers);

            cursor.GotoNext(instr => instr.MatchCall("Monocle.Entity", "RemoveSelf"));
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_Platform_DestroyStaticMovers);
        }

        public static void PatchCrushBlockFirstAlarm(MethodDefinition method) {
            ILCursor cursor = new ILCursor(new ILContext(method));

            // this.currentMoveLoopSfx.Stop(true);
            // TO
            // this.currentMoveLoopSfx?.Stop(true);
            cursor.GotoNext(MoveType.After, instr => instr.MatchLdfld("Celeste.CrushBlock", "currentMoveLoopSfx"));
            Instruction instrPop = cursor.Clone().GotoNext(instr => instr.MatchPop()).Next;
            cursor.Emit(OpCodes.Dup);
            cursor.Emit(OpCodes.Brfalse_S, instrPop);
        }

        public static void PatchLookoutUpdate(ILContext context, CustomAttribute attrib) {
            FieldReference f_talk = context.Method.DeclaringType.FindField("talk");
            FieldReference f_TalkComponent_UI = context.Method.Module.GetType("Celeste.TalkComponent").FindField("UI");
            FieldReference f_Entity_Visible = context.Method.Module.GetType("Monocle.Entity").FindField("Visible");

            // Remove the following, saving the MethodReference for CollideCheck<Solid>
            // if (this.talk == null || !CollideCheck<Solid>())
            //     return;
            // this.Remove((Component) this.talk);
            // this.talk = (TalkComponent) null;
            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(MoveType.AfterLabel, instr => instr.OpCode == OpCodes.Ldarg_0,
                instr => instr.MatchLdfld("Celeste.Lookout", "talk"));

            MethodReference m_CollideCheck = cursor.Clone().GotoNext(instr => instr.MatchCall("Monocle.Entity", "CollideCheck")).Next.Operand as MethodReference;
            cursor.Next.OpCode = OpCodes.Nop; // This instr may have a break instruction pointing to it
            while (cursor.TryGotoNext(instr => instr.Next != null))
                cursor.Remove();


            // Reset to the top and insert
            // if (talk.UI != null) {
            //     talk.UI.Visible = !CollideCheck<Solid>();
            // }
            cursor.Goto(0);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_talk);
            cursor.Emit(OpCodes.Ldfld, f_TalkComponent_UI);
            cursor.Emit(OpCodes.Brfalse_S, cursor.Next);

            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_talk);
            cursor.Emit(OpCodes.Ldfld, f_TalkComponent_UI);
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_CollideCheck);
            cursor.Emit(OpCodes.Ldc_I4_0);
            cursor.Emit(OpCodes.Ceq);
            cursor.Emit(OpCodes.Stfld, f_Entity_Visible);
        }

        public static void PatchDecalUpdate(ILContext context, CustomAttribute attrib) {
            FieldDefinition f_hideRange = context.Method.DeclaringType.FindField("hideRange");
            FieldDefinition f_showRange = context.Method.DeclaringType.FindField("showRange");

            ILCursor cursor = new ILCursor(context);
            cursor.GotoNext(instr => instr.MatchLdcR4(32f));
            cursor.Remove();
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_hideRange);

            cursor.GotoNext(instr => instr.MatchLdcR4(48f));
            cursor.Remove();
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Ldfld, f_showRange);
        }

        public static void PatchAreaCompleteMusic(ILContext context, CustomAttribute attrib) {
            MethodDefinition m_playCustomCompleteScreenMusic = context.Method.DeclaringType.FindMethod("System.Boolean playCustomCompleteScreenMusic()");

            ILCursor cursor = new ILCursor(context);

            // we want to inject code just after RunThread.Start that calls playCustomCompleteScreenMusic(),
            // and sends execution to Audio.SetAmbience(null) if it returned true (skipping over the vanilla code playing endscreen music).
            cursor.GotoNext(MoveType.After, instr => instr.MatchCall("Celeste.RunThread", "Start"));
            Instruction breakTarget = cursor.Clone().GotoNext(
                instr => instr.MatchLdnull(),
                instr => instr.OpCode == OpCodes.Ldc_I4_1,
                instr => instr.MatchCall("Celeste.Audio", "SetAmbience")
            ).Next;
            cursor.Emit(OpCodes.Ldarg_0);
            cursor.Emit(OpCodes.Call, m_playCustomCompleteScreenMusic);
            cursor.Emit(OpCodes.Brtrue_S, breakTarget);
        }

        public static void PostProcessor(MonoModder modder) {
            // Patch CrushBlock::AttackSequence's first alarm delegate manually because how would you even annotate it?
            PatchCrushBlockFirstAlarm(modder.Module.GetType("Celeste.CrushBlock/<>c__DisplayClass41_0").FindMethod("<AttackSequence>b__1"));

            // Patch previously registered AreaCompleteCtors and LevelExitRoutines _in that order._
            foreach (MethodDefinition method in AreaCompleteCtors)
                PatchAreaCompleteCtor(method);
            foreach (MethodDefinition method in LevelExitRoutines)
                PatchLevelExitRoutine(method);

            foreach (TypeDefinition type in modder.Module.Types) {
                PostProcessType(modder, type);
            }
        }

        private static void PostProcessType(MonoModder modder, TypeDefinition type) {
            foreach (MethodDefinition method in type.Methods) {
                PostProcessMethod(modder, method);
                method.FixShortLongOps();
            }

            foreach (TypeDefinition nested in type.NestedTypes)
                PostProcessType(modder, nested);
        }

        private static void PostProcessMethod(MonoModder modder, MethodDefinition method) {
            // Find all FMOD-related extern methods and stub them.
            if (FMODStub &&
                !method.HasBody && method.HasPInvokeInfo && (
                method.PInvokeInfo.Module.Name == "fmod" ||
                method.PInvokeInfo.Module.Name == "fmodstudio")
            ) {
                MonoModRule.Modder.Log($"[Everest] [FMODStub] Stubbing {method.FullName} -> {method.PInvokeInfo.Module.Name}::{method.PInvokeInfo.EntryPoint}");

                if (method.HasPInvokeInfo)
                    method.PInvokeInfo = null;
                method.IsManaged = true;
                method.IsIL = true;
                method.IsNative = false;
                method.PInvokeInfo = null;
                method.IsPreserveSig = false;
                method.IsInternalCall = false;
                method.IsPInvokeImpl = false;

                MethodBody body = method.Body = new MethodBody(method);
                body.InitLocals = true;
                ILProcessor il = body.GetILProcessor();

                for (int i = 0; i < method.Parameters.Count; i++) {
                    ParameterDefinition param = method.Parameters[i];
                    if (param.IsOut || param.IsReturnValue) {
                        // il.Emit(OpCodes.Ldarg, i);
                        // il.EmitDefault(param.ParameterType, true);
                    }
                }

                il.EmitDefault(method.ReturnType ?? method.Module.TypeSystem.Void);
                il.Emit(OpCodes.Ret);
                return;
            }

        }

        public static void EmitDefault(this ILProcessor il, TypeReference t, bool stind = false, bool arrayEmpty = true) {
            if (t == null) {
                il.Emit(OpCodes.Ldnull);
                if (stind)
                    il.Emit(OpCodes.Stind_Ref);
                return;
            }

            if (t.MetadataType == MetadataType.Void)
                return;

            if (t.IsArray && arrayEmpty) {
                // TODO: Instead of emitting a null array, emit an empty array.
            }

            int var = 0;
            if (!stind) {
                var = il.Body.Variables.Count;
                il.Body.Variables.Add(new VariableDefinition(t));
                il.Emit(OpCodes.Ldloca, var);
            }
            il.Emit(OpCodes.Initobj, t);
            if (!stind)
                il.Emit(OpCodes.Ldloc, var);
        }

    }
}
