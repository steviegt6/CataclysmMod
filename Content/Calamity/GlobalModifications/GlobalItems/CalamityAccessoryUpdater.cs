using CalamityMod;
using CalamityMod.Items.Accessories;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Calamity.GlobalModifications.ModPlayers;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    [ModDependency("CalamityMod")]
    public class CalamityAccessoryUpdater : CataclysmGlobalItem
    {
        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.ObsidianHorseshoe:
                case ItemID.ObsidianShield:
                case ItemID.AnkhShield:
                case ItemID.ObsidianWaterWalkingBoots:
                case ItemID.LavaWaders:
                case ItemID.ObsidianSkull:
                    player.GetModPlayer<CalamityCataclysmPlayer>().ObsidianSkullHeatRes = true;
                    break;
            }

            if (item.type == ModContent.ItemType<Sponge>() || item.type == ModContent.ItemType<TheAbsorber>())
                player.Calamity().roverDrive = true;
        }
    }
}