using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ModLoader;
using ThoriumMod.Items.Placeable;

namespace CataclysmMod.Content.Thorium.GlobalModifications.GlobalItems
{
    [ModDependency("ThoriumMod")]
    public class ThoriumItemDefaultsModifier : CataclysmGlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<BlackStainedGlass>() ||
                item.type == ModContent.ItemType<CyanStainedGlass>() ||
                item.type == ModContent.ItemType<PinkStainedGlass>())
                item.value = Utils.Clamp(item.value, Item.sellPrice(silver: 5), int.MaxValue);
        }
    }
}