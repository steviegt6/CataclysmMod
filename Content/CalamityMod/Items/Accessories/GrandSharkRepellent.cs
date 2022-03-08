#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using Calamity = CalamityMod;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.CalamityMod.Items.Accessories
{
    [AddonContent(typeof(CalamityModAddon))]
    public class GrandSharkRepellent : ModItem
    {
        public override void SetStaticDefaults() =>
            Tooltip.SetDefault("Stops the Grand Sand Shark from spawning when you kill 10 sand sharks" +
                               "\nEquipping this will reset the sand shark counter to 0");

        public override void SetDefaults()
        {
            item.Size = new Vector2(20f, 30f);
            item.accessory = true;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(gold: 1, silver: 25);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) =>
            Calamity::CalamityMod.sharkKillCount = 0;
    }
}