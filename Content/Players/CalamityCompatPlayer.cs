﻿using CalamityMod.Items.LoreItems;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Players
{
    public class CalamityCompatPlayer : ModPlayer
    {
        public bool playerHasObsidianSkullOrTree;

        public override void ResetEffects() => playerHasObsidianSkullOrTree = false;

        public override void PostUpdateEquips()
        {
            if (!CataclysmConfig.Instance.loreItemsInPiggyBank)
                return;

            foreach (Item item in player.bank.item)
                if (item.modItem is LoreItem)
                {
                    item.favorited = true;
                    ItemLoader.UpdateInventory(item, player);
                }
        }
    }
}