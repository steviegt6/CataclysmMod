using System.Collections.Generic;
using System.Linq;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Tools;
using CataclysmMod.Common.Utilities;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class TooltipModificationsItem : GlobalItem
    {
        private readonly struct PickaxeTooltipReplacementData
        {
            public readonly string tooltipToMatch;
            public readonly string tooltipReplacement;
            public readonly int itemID;

            public PickaxeTooltipReplacementData(string tooltipToMatch, string tooltipReplacement, int itemID = ItemID.None)
            {
                this.tooltipToMatch = tooltipToMatch;
                this.tooltipReplacement = tooltipReplacement;
                this.itemID = itemID;
            }
        }

        private static List<PickaxeTooltipReplacementData> TooltipReplacements;

        internal static void Initialize()
        {
            TooltipReplacements = new List<PickaxeTooltipReplacementData>
            {
                // Nightmare & Deathbringer
                new PickaxeTooltipReplacementData("Able to mine Hellstone", "Capable of mining Hellstone and Aerialite"),

                // Picksaw
                new PickaxeTooltipReplacementData("Capable of mining Lihzahrd Bricks", "Capable of mining Lihzahrd Bricks and Astral Ore", ItemID.Picksaw),

                // Seismic Hampick
                new PickaxeTooltipReplacementData("Capable of mining Lihzahrd Bricks", "Capable of mining Lihzahrd Bricks, Astral Ore, and Scoria", ModContent.ItemType<FlamebeakHampick>()),

                // Genesis Pickaxe
                new PickaxeTooltipReplacementData("Can mine Uelibloom Ore", "Capable of mining Uelibloom Ore and Exodium Clusters"),

                // Gold & Platinum
                new PickaxeTooltipReplacementData("Can mine Meteorite", "Capable of mining Meteorite and Sea Prisms")
            };
        }
        
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            foreach (PickaxeTooltipReplacementData tooltipData in TooltipReplacements)
            {
                TooltipLine tooltip = tooltips.FirstOrDefault(l => l.text == tooltipData.tooltipToMatch);

                if (tooltip != null && (item.type == tooltipData.itemID || tooltipData.itemID == ItemID.None))
                    tooltip.text = tooltipData.tooltipReplacement;
            }

            switch (item.type)
            {
                case ItemID.AdamantitePickaxe:
                case ItemID.TitaniumPickaxe:
                    tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred", "Capable of mining Cryonic and Charred Ore"));
                    break;

                case ItemID.PickaxeAxe:
                case ItemID.Drax:
                case ItemID.ChlorophytePickaxe:
                    tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickPerennial","Capable of mining Perennial Ore"));
                    break;

                case ItemID.SolarFlarePickaxe:
                case ItemID.VortexPickaxe:
                case ItemID.NebulaPickaxe:
                case ItemID.StardustPickaxe:
                    tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickExodium", "Capable of mining Exodium Clusters"));
                    break;
            }

            if (item.type == ModContent.ItemType<RampartofDeities>() || item.type == ModContent.ItemType<DeificAmulet>())
                tooltips.Add(new TooltipLine(mod, $"{mod.Name}:BandRegen", "Provides life regeneration"));
        }
    }
}