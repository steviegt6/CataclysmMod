using CataclysmMod.Content.GlobalModifications.Players;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class ObsidianSkullHeatResUpdateItem : GlobalItem
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
                    player.GetModPlayer<CalamityCompatPlayer>().playerHasObsidianSkullOrTree = true;
                    break;
            }
        }
    }
}