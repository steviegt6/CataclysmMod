using CalamityMod;
using CalamityMod.Items.Fishing.SulphurCatches;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class AbyssalCrateGlobal : GlobalItem
    {
        public override void RightClick(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<AbyssalCrate>())
                DropHelper.DropItemChance(player, ModContent.ItemType<SulphurousShell>(), 0.1f, 1, 1);
        }
    }
}
