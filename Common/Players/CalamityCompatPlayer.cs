using CalamityMod.Items.LoreItems;
using CataclysmMod.Common.Configs;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Players
{
    public class CalamityCompatPlayer : ModPlayer
    {
        public override void PostUpdateEquips()
        {
            if (CalamityChangesConfig.Instance.loreItemsInPiggyBank)
                for (int i = 0; i < player.bank.item.Length; i++)
                    if (player.bank.item[i].modItem is LoreItem)
                    {
                        player.bank.item[i].favorited = true;
                        ItemLoader.UpdateInventory(player.bank.item[i], player);
                    }
        }
    }
}