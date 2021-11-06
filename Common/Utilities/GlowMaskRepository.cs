﻿using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Content.Split.Items.Accessories;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class GlowMaskRepository
    {
        public const string TextureSearch = "CataclysmToRemove_";

        public static Dictionary<string, int> GlowMasks;

        public static void Load()
        {
            if (Main.dedServ)
                return;

            GlowMasks = new Dictionary<string, int>();

            List<Texture2D> glowMasks = Main.glowMaskTexture.ToList();
            int count = glowMasks.Count;

            foreach ((string name, Texture2D texture) in GetGlowMasks())
            {
                GlowMasks.Add(name, count++);
                texture.Name = TextureSearch + texture;
                glowMasks.Add(texture);
            }

            Main.glowMaskTexture = glowMasks.ToArray();
        }

        public static void Unload()
        {
            if (Main.dedServ)
                return;

            Main.glowMaskTexture = Main.glowMaskTexture.Where(x => !x.Name.StartsWith(TextureSearch)).ToArray();
            GlowMasks.Clear();
            GlowMasks = null;
        }

        private static IEnumerable<(string, Texture2D)> GetGlowMasks()
        {
            yield return (
                nameof(PharaohsFear),
                ModContent.GetTexture($"CataclysmMod/Content/Split/Items/Accessories/{nameof(PharaohsFear)}_Glow")
            );
        }
    }
}