using CalamityMod.Items.LoreItems;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Players
{
    public class LoreItemPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            for (int i = 0; i < player.bank.item.Length; i++)
                if (player.bank.item[i].modItem is LoreItem)
                {
                    player.bank.item[i].favorited = true;
                    ItemLoader.UpdateInventory(player.bank.item[i], player);
                }
        }
    }
}
