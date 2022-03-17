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
            SetShopItem(
                ref shop,
                ref nextSlot,
                ItemID.GuideVoodooDoll,
                Main.hardMode,
                Item.sellPrice(gold: 20)
            );
        
        public static void SetShopItem(
            ref Chest shop,
            ref int nextSlot,
            int itemID,
            bool condition = true,
            int? price = null,
            bool ignoreDiscount = false)
        {
            if (!condition)
                return;
            
            shop.item[nextSlot].SetDefaults(itemID);
            
            if (price.HasValue)
            {
                shop.item[nextSlot].shopCustomPrice = price;
                
                if (Main.LocalPlayer.discount && !ignoreDiscount)
                {
                    Item item = shop.item[nextSlot];
                    int? shopCustomPrice = shop.item[nextSlot].shopCustomPrice;

                    if (shopCustomPrice != null)
                        shopCustomPrice = (int) (shopCustomPrice.Value * 0.8f);
                    
                    item.shopCustomPrice = shopCustomPrice;
                }
            }
            ++nextSlot;
        }
    }
}