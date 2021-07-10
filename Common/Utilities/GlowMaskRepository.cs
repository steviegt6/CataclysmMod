using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Content.Split.Items.Accessories;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class GlowMaskRepository
    {
        public static Dictionary<string, int> GlowMasks;

        public static void Load()
        {
            GlowMasks = new Dictionary<string, int>();

            List<Texture2D> glowMasks = Main.glowMaskTexture.ToList();
            int count = glowMasks.Count;

            GlowMasks.Add(nameof(PharaohsFear), count++);
            glowMasks.Add(ModContent.GetTexture($"CataclysmMod/Content/Split/Items/Accessories/{nameof(PharaohsFear)}_Glow"));

            Main.glowMaskTexture = glowMasks.ToArray();
        }
    }
}