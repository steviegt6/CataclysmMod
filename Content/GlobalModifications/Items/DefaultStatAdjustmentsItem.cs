using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class DefaultStatAdjustmentsItem : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.SpiderMask:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 3;
                    break;

                case ItemID.SpiderBreastplate:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 2;
                    break;

                case ItemID.SpiderGreaves:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 1;
                    break;

                case ItemID.GuideVoodooDoll:
                case ItemID.ClothierVoodooDoll:
                    if (item.maxStack < 20 && CataclysmConfig.Instance.voodooDollStackIncrease)
                        item.maxStack = 20;
                    break;
            }

            if (CataclysmConfig.Instance.basherScale && item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;

            if (CataclysmConfig.Instance.sulphurSkinPotionPriceNerf &&
                item.type == ModContent.ItemType<SulphurskinPotion>())
                item.value = Item.sellPrice(silver: 2);

            if (CataclysmConfig.Instance.infinityDontConsumeAmmo && item.type == ModContent.ItemType<Infinity>())
                item.damage = 25;
        }
    }
}
