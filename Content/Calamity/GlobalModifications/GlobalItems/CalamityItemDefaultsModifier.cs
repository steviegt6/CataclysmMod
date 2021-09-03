using CalamityMod.Items.Accessories;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    public class CalamityItemDefaultsModifier : CalamityGlobalItemBase
    {
        // TODO: move this to default?
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.SpiderMask:
                    item.defense += 3;
                    break;

                case ItemID.SpiderBreastplate:
                    item.defense += 2;
                    break;

                case ItemID.SpiderGreaves:
                    item.defense += 1;
                    break;

                case ItemID.GuideVoodooDoll:
                case ItemID.ClothierVoodooDoll:
                    item.maxStack = Utils.Clamp(item.maxStack, 20, int.MaxValue);
                    break;
            }

            // obviously calamity
            if (item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;

            if (item.type == ModContent.ItemType<SulphurskinPotion>())
                item.value = Item.sellPrice(silver: 2);

            if (item.type == ModContent.ItemType<Infinity>())
                item.damage = 25;

            if (item.type == ModContent.ItemType<RampartofDeities>() ||
                item.type == ModContent.ItemType<DeificAmulet>())
                item.lifeRegen += 2;
        }
    }
}