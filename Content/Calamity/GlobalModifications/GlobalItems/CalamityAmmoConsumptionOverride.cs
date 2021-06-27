using System.Collections.Generic;
using CalamityMod.Items.Weapons.Ranged;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    [ModDependency("CalamityMod")]
    public class CalamityAmmoConsumptionOverride : CataclysmGlobalItem
    {
        public List<int> ExemptItems => new List<int>
        {
            ModContent.ItemType<Infinity>()
        };

        public override bool ConsumeAmmo(Item item, Player player) =>
            !ExemptItems.Contains(item.type) && base.ConsumeAmmo(item, player);
    }
}