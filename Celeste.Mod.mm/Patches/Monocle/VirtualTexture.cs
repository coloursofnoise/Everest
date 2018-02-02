﻿#pragma warning disable CS0626 // Method, operator, or accessor is marked external and has no attributes on it

using Celeste.Mod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoMod;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Monocle {
    class patch_VirtualTexture : patch_VirtualAsset {

        // We're effectively in VirtualAsset, but still need to "expose" private fields to our mod.
        public string Path { get; private set; }
        // This is public, but we don't build upon the original type. MonoMod knows what to do, though.
        public Texture2D Texture;

        public AssetMetadata Metadata { get; private set; }

        // This _should_ work, but hell, this MonoModConstructor usage syntax went untested for ages. -ade
        [MonoModConstructor]
        internal patch_VirtualTexture(AssetMetadata metadata) {
            Metadata = metadata;
            Name = metadata.PathRelative;
            Reload();
        }

        internal extern void orig_Reload();
        internal override void Reload() {
            if (Metadata == null) {
                orig_Reload();
                return;
            }

            Unload();

            using (Stream stream = Metadata.Stream)
                Texture = Texture2D.FromStream(Celeste.Celeste.Instance.GraphicsDevice, stream);
            Width = Texture.Width;
            Height = Texture.Height;
        }

    }
}
