using CalamityMod.NPCs;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalNpcs
{
    public class CalamityShopModifier : CalamityGlobalNpcBase
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