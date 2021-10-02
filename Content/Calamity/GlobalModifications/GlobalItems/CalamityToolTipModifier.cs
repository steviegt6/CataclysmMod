using System.Collections.Generic;
using System.Linq;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Tools;
using CataclysmMod.Common.DataStructures;
using CataclysmMod.Common.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    public class CalamityToolTipModifier : CalamityGlobalItemBase
    {
        public List<ToolTipReplacement> ToolTipReplacements => new List<ToolTipReplacement>
        {
            // Nightmare & Deathbringer
            new ToolTipReplacement("Able to mine Hellstone", "Capable of mining Hellstone and Aerialite"),

            // Picksaw
            new ToolTipReplacement("Capable of mining Lihzahrd Bricks",
                "Capable of mining Lihzahrd Bricks and Astral Ore", ItemID.Picksaw),

            // Seismic Hampick
            new ToolTipReplacement("Capable of mining Lihzahrd Bricks",
                "Capable of mining Lihzahrd Bricks, Astral Ore, and Scoria",
                ModContent.ItemType<FlamebeakHampick>()),

            // Genesis Pickaxe
            new ToolTipReplacement("Can mine Uelibloom Ore",
                "Capable of mining Uelibloom Ore and Exodium Clusters"),

            // Gold & Platinum
            new ToolTipReplacement("Can mine Meteorite", "Capable of mining Meteorite and Sea Prisms")
        };

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (ToolTipReplacement tooltipData in ToolTipReplacements)
            {
                TooltipLine tooltip = tooltips.FirstOrDefault(l => l.text == tooltipData.Match);

                if (tooltip != null && (item.type == tooltipData.ItemId || tooltipData.ItemId == ItemID.None))
                    tooltip.text = tooltipData.Replacement;
            }

            switch (item.type)
            {
                case ItemID.AdamantitePickaxe:
                case ItemID.TitaniumPickaxe:
                    ToolTipUtilities.AddToolTip(tooltips,
                        new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred",
                            "Capable of mining Cryonic and Charred Ore"));
                    break;

                case ItemID.PickaxeAxe:
                case ItemID.Drax:
                case ItemID.ChlorophytePickaxe:
                    ToolTipUtilities.AddToolTip(tooltips,
                        new TooltipLine(mod, $"{mod.Name}:PickPerennial", "Capable of mining Perennial Ore"));
                    break;

                case ItemID.SolarFlarePickaxe:
                case ItemID.VortexPickaxe:
                case ItemID.NebulaPickaxe:
                case ItemID.StardustPickaxe:
                    ToolTipUtilities.AddToolTip(tooltips,
                        new TooltipLine(mod, $"{mod.Name}:PickExodium", "Capable of mining Exodium Clusters"));
                    break;
            }

            if (item.type == ModContent.ItemType<RampartofDeities>() ||
                item.type == ModContent.ItemType<DeificAmulet>())
                ToolTipUtilities.AddToolTip(tooltips,
                    new TooltipLine(mod, $"{mod.Name}:BandRegen", "Provides life regeneration"));

            if (item.type == ModContent.ItemType<Sponge>() || item.type == ModContent.ItemType<TheAbsorber>())
                ToolTipUtilities.AddToolTip(tooltips, new TooltipLine(mod, $"{mod.Name}:RoverDrive",
                    "Activates a protective shield that grants 10 defense for 10 seconds" +
                    "\nThe shield then dissipates and recharges for 20 seconds before being reactivated"));
        }
    }
}