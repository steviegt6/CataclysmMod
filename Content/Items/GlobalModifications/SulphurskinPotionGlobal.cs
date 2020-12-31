using CalamityMod.Items.Potions;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class SulphurskinPotionGlobal : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CalamityChangesConfig.Instance.sulphurSkinPotionPriceNerf && item.type == ModContent.ItemType<SulphurskinPotion>())
                item.value = Item.sellPrice(silver: 2);
        }
    }
}