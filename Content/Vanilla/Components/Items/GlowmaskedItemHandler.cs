#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Vanilla.Components.Items
{
    public class GlowmaskedItemHandler : GlobalItem
    {
        private static Dictionary<string, short> GlowmaskCollection = new Dictionary<string, short>();
        
        public static void Unload()
        {
            Main.glowMaskTexture = Main.glowMaskTexture.Where(x => !x?.Name.StartsWith("$") ?? true).ToArray();
            
            GlowmaskCollection.Clear();
        }

        public short GetGlowmask(IGlowmaskedItemComponent glowmaskedItem)
        {
            if (GlowmaskCollection.ContainsKey(glowmaskedItem.GlowmaskPath))
                return GlowmaskCollection[glowmaskedItem.GlowmaskPath];

            short count = (short) Main.glowMaskTexture.Length;
            Texture2D texture = ModContent.GetTexture(glowmaskedItem.GlowmaskPath);

            texture.Name = '$' + texture.Name;

            List<Texture2D> list = Main.glowMaskTexture.ToList();
            list.Add(texture);
            Main.glowMaskTexture = list.ToArray();
            GlowmaskCollection.Add(glowmaskedItem.GlowmaskPath, count);
            return count;
        }

        public override void SetDefaults(Item item)
        {
            base.SetDefaults(item);

            if (item.modItem is IGlowmaskedItemComponent glowmaskedItem)
                item.glowMask = GetGlowmask(glowmaskedItem);
        }
    }
}