using CalamityMod.NPCs;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class NPCShopModificationGlobal : GlobalNPC
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

        public void SetupWizardShop(Chest shop, ref int nextSlot)
        {
            if (CalamityChangesConfig.Instance.wizardGuideVoodooDoll)
                CalamityGlobalTownNPC.SetShopItem(ref shop, ref nextSlot, ItemID.GuideVoodooDoll, Main.hardMode, Item.sellPrice(gold: 20));
        }
    }
}