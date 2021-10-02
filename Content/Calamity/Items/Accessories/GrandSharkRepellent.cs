using CataclysmMod.Content.Default.Items;
using CataclysmMod.Core.ModCompatibility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.Calamity.Items.Accessories
{
    [ModDependency("CalamityMod")]
    public class GrandSharkRepellent : CataclysmItem
    {
        public override void SetStaticDefaults() =>
            Tooltip.SetDefault("Stops the Grand Sand Shark from spawning when you kill 10 sand sharks" +
                               "\nEquipping this will reset the sand shark counter to 0");

        public override void SetDefaults()
        {
            item.Size = new Vector2(20f, 30f);
            item.accessory = true;
            item.rare = ItemRarityID.Lime;
            item.value = Item.sellPrice(gold: 1, silver: 25);
        }

        public override void UpdateAccessory(Player player, bool hideVisual) =>
            CalamityMod.CalamityMod.sharkKillCount = 0;
    }
}