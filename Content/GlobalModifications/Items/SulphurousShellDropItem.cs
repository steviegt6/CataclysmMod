using CalamityMod;
using CalamityMod.Items.Fishing.SulphurCatches;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.Items;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class SulphurousShellDropItem : GlobalItem
    {
        public override void RightClick(Item item, Player player)
        {
            if (CataclysmConfig.Instance.sulphurousShell && item.type == ModContent.ItemType<AbyssalCrate>())
                DropHelper.DropItemChance(player, ModContent.ItemType<SulphurousShell>(), 0.1f, 1, 1);
        }
    }
}
