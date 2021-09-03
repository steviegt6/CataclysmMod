using CalamityMod;
using CalamityMod.Items.Fishing.SulphurCatches;
using CataclysmMod.Content.Calamity.Items.Tools;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    public class CalamityDropModifier : CalamityGlobalItemBase
    {
        public override void RightClick(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<AbyssalCrate>())
                DropHelper.DropItemChance(player, ModContent.ItemType<SulphurousShell>(), 0.1f, 1, 1);
        }
    }
}