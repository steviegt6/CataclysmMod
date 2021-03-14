using CalamityMod;
using CalamityMod.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class SpongeAbsorberRoverDrive : GlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            if (item.type == ModContent.ItemType<Sponge>() || item.type == ModContent.ItemType<TheAbsorber>())
                player.Calamity().roverDrive = true;
        }
    }
}
