#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Placeable;

namespace CataclysmMod.Content.Thorium.Globals.Items
{
    [AddonContent(typeof(ThoriumModAddon))]
    public class ThoriumSetDefaultsModifier : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<BlackStainedGlass>() ||
                item.type == ModContent.ItemType<CyanStainedGlass>() ||
                item.type == ModContent.ItemType<PinkStainedGlass>())
                item.value = Utils.Clamp(item.value, Item.sellPrice(silver: 5), int.MaxValue);
        }
    }
}