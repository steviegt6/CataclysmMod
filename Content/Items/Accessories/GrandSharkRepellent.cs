using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.Accessories
{
    public class GrandSharkRepellent : ModItem
    {
        public override void SetStaticDefaults() => Tooltip.SetDefault("Stops the Grand Sand Shark from spawning when you kill 10 sand sharks\nEquipping this will reset the sand shark counter to 0");

        public override void SetDefaults()
        {
            item.width = 20;
            item.height = 26;
            item.accessory = true;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(gold: 1, silver: 25);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) => CalamityMod.CalamityMod.sharkKillCount = 0;
    }
}