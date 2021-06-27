using CalamityMod.NPCs;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalNpcs
{
    [ModDependency("CalamityMod")]
    public class CalamityShopModifier : CataclysmGlobalNpc
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