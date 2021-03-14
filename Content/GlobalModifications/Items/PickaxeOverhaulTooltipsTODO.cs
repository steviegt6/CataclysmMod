using System.Collections.Generic;
using System.Linq;
using CalamityMod.Items.Tools;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

// TODO: Split into separate files in its own folder?
namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class PickaxeOverhaulTooltipsTODO : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");

            if (!CataclysmConfig.Instance.pickaxeTooltips)
                return;

            if (tooltip != null)
            {
                switch (item.type)
                {
                    case ItemID.DeathbringerPickaxe:
                    case ItemID.NightmarePickaxe:
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineAerialite");
                        break;
                    case ItemID.Picksaw:
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineAstral");
                        break;
                }

                if (item.type == ModContent.ItemType<FlamebeakHampick>())
                    tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineScoriaAstral");

                if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe ||
                    item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe ||
                    item.type == ModContent.ItemType<GallantPickaxe>())
                    tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineExodium");
            }
            else
            {
                switch (item.type)
                {
                    case ItemID.GoldPickaxe:
                    case ItemID.PlatinumPickaxe:
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickSeaPrism",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineSeaPrism")));
                        break;
                    case ItemID.AdamantitePickaxe:
                    case ItemID.TitaniumPickaxe:
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineCryonicCharred")));
                        break;
                    case ItemID.PickaxeAxe:
                    case ItemID.Drax:
                    case ItemID.ChlorophytePickaxe:
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickPerennial",
                            LangUtils.GetCataclysmTextValue("Tooltips.Perennial")));
                        break;
                    case ItemID.SolarFlarePickaxe:
                    case ItemID.VortexPickaxe:
                    case ItemID.NebulaPickaxe:
                    case ItemID.StardustPickaxe:
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickExodium",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineExodium")));
                        break;
                }
            }
        }
    }
}