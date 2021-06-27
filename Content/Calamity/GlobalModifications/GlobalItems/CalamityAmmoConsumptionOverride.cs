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
        // Instanced field because static would cause a static constructor call regardless of loaded mods
        public List<int> ExemptItems { get; } = new List<int>
        {
            ModContent.ItemType<Infinity>()
        };

        public override bool ConsumeAmmo(Item item, Player player) =>
            !ExemptItems.Contains(item.type) && base.ConsumeAmmo(item, player);
    }
}