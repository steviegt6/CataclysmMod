using CalamityMod.Items.Weapons.Ranged;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class AmmoConsumptionAdjustmentItem : GlobalItem
    {
        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<Infinity>() && CataclysmConfig.Instance.infinityDontConsumeAmmo)
                return false;

            return base.ConsumeAmmo(item, player);
        }
    }
}
