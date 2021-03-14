﻿using CalamityMod.NPCs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.NPCs
{
    public class SetupShopHandlerNPC : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Wizard:
                    SetupWizardShop(shop, ref nextSlot);
                    break;
            }
        }

        private static void SetupWizardShop(Chest shop, ref int nextSlot) =>
            CalamityGlobalTownNPC.SetShopItem(ref shop, ref nextSlot, ItemID.GuideVoodooDoll, Main.hardMode,
                Item.sellPrice(gold: 20));
    }
}