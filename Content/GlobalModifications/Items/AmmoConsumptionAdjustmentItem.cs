using CalamityMod.Items.Weapons.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class AmmoConsumptionAdjustmentItem : GlobalItem
    {
        public override bool ConsumeAmmo(Item item, Player player) => item.type != ModContent.ItemType<Infinity>() && base.ConsumeAmmo(item, player);
    }
}