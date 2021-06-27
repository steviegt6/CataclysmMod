using CalamityMod.Items.LoreItems;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.ModPlayers
{
    [ModDependency("CalamityMod")]
    public class CalamityCataclysmPlayer : CataclysmPlayer
    {
        public bool ObsidianSkullHeatRes;

        public override void ResetEffects()
        {
            ObsidianSkullHeatRes = false;
        }

        public override void PostUpdateEquips()
        {
            foreach (Item item in player.bank.item)
                if (item.modItem is LoreItem)
                {
                    item.favorited = true;
                    ItemLoader.UpdateInventory(item, player);
                }
        }
    }
}