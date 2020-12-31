using CalamityMod.Items.Tools;
using CataclysmMod.Common.Configs;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class PickaxeTooltipsGlobal : GlobalItem
    {
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");

            if (CalamityChangesConfig.Instance.pickaxeTooltips)
            {
                if (tooltip != null)
                {
                    if (item.type == ItemID.DeathbringerPickaxe || item.type == ItemID.NightmarePickaxe)
                        tooltip.text += "\n" + Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineAerialite");

                    if (item.type == ItemID.Picksaw)
                        tooltip.text += "\n" + Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineAstral");

                    if (item.type == ModContent.ItemType<FlamebeakHampick>())
                        tooltip.text += "\n" + Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineScoriaAstral");

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe || item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe || item.type == ModContent.ItemType<GallantPickaxe>())
                        tooltip.text += "\n" + Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineExodium");
                }
                else
                {
                    if (item.type == ItemID.GoldPickaxe || item.type == ItemID.PlatinumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickSeaPrism", Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineSeaPrism")));

                    if (item.type == ItemID.AdamantitePickaxe || item.type == ItemID.TitaniumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred", Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineCryonicCharred")));

                    if (item.type == ItemID.PickaxeAxe || item.type == ItemID.Drax || item.type == ItemID.ChlorophytePickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickPerennial", Language.GetTextValue("Mods.CataclysmMod.Tooltips.MinePerennial")));

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe || item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickExodium", Language.GetTextValue("Mods.CataclysmMod.Tooltips.MineExodium")));
                }
            }
        }
    }
}