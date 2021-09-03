using System.Collections.Generic;
using CalamityMod.Items.Weapons.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    public class CalamityAmmoConsumptionOverride : CalamityGlobalItemBase
    {
        public List<int> ExemptItems => new List<int>
        {
            ModContent.ItemType<Infinity>()
        };

        public override bool ConsumeAmmo(Item item, Player player) =>
            !ExemptItems.Contains(item.type) && base.ConsumeAmmo(item, player);
    }
}