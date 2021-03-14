using CalamityMod.Items.LoreItems;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Players
{
    public class CalamityCompatPlayer : ModPlayer
    {
        public bool playerHasObsidianSkullOrTree;

        public override void ResetEffects() => playerHasObsidianSkullOrTree = false;

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