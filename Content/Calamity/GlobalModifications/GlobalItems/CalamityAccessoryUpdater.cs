using CalamityMod;
using CalamityMod.Items.Accessories;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    [ModDependency("CalamityMod")]
    public class CalamityAccessoryUpdater : CataclysmGlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<Sponge>() || item.type == ModContent.ItemType<TheAbsorber>())
                player.Calamity().roverDrive = true;
        }
    }
}