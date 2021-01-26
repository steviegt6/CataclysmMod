using CalamityMod;
using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Melee;
using CataclysmMod.Common.Configs;
using CataclysmMod.Utilities;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class CalamityCompatGlobalItem : GlobalItem
    {
        public override bool Autoload(ref string name) => CataclysmMod.Instance.Calamity != null;

        public override void RightClick(Item item, Player player)
        {
            if (CalamityChangesConfig.Instance.sulphurousShell && item.type == ModContent.ItemType<AbyssalCrate>())
                DropHelper.DropItemChance(player, ModContent.ItemType<SulphurousShell>(), 0.1f, 1, 1);
        }

        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.SpiderMask:
                    if (CalamityChangesConfig.Instance.spiderArmorBuff)
                        item.defense += 3;
                    break;

                case ItemID.SpiderBreastplate:
                    if (CalamityChangesConfig.Instance.spiderArmorBuff)
                        item.defense += 2;
                    break;

                case ItemID.SpiderGreaves:
                    if (CalamityChangesConfig.Instance.spiderArmorBuff)
                        item.defense += 1;
                    break;
            }

            if (CalamityChangesConfig.Instance.basherScale && item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;

            if (CalamityChangesConfig.Instance.sulphurSkinPotionPriceNerf && item.type == ModContent.ItemType<SulphurskinPotion>())
                item.value = Item.sellPrice(silver: 2);
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");

            if (CalamityChangesConfig.Instance.pickaxeTooltips)
            {
                if (tooltip != null)
                {
                    if (item.type == ItemID.DeathbringerPickaxe || item.type == ItemID.NightmarePickaxe)
                        tooltip.text += "\n" + LangUtils.GetCalamityTextValue("Tooltips.MineAerialite");

                    if (item.type == ItemID.Picksaw)
                        tooltip.text += "\n" + LangUtils.GetCalamityTextValue("Tooltips.MineAstral");

                    if (item.type == ModContent.ItemType<FlamebeakHampick>())
                        tooltip.text += "\n" + LangUtils.GetCalamityTextValue("Tooltips.MineScoriaAstral");

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe || item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe || item.type == ModContent.ItemType<GallantPickaxe>())
                        tooltip.text += "\n" + LangUtils.GetCalamityTextValue("Tooltips.MineExodium");
                }
                else
                {
                    if (item.type == ItemID.GoldPickaxe || item.type == ItemID.PlatinumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickSeaPrism", LangUtils.GetCalamityTextValue("Tooltips.MineSeaPrism")));

                    if (item.type == ItemID.AdamantitePickaxe || item.type == ItemID.TitaniumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred", LangUtils.GetCalamityTextValue("Tooltips.MineCryonicCharred")));

                    if (item.type == ItemID.PickaxeAxe || item.type == ItemID.Drax || item.type == ItemID.ChlorophytePickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickPerennial", LangUtils.GetCalamityTextValue("Tooltips.Perennial")));

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe || item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickExodium", LangUtils.GetCalamityTextValue("Tooltips.MineExodium")));
                }
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.SpiderMask && body.type == ItemID.SpiderBreastplate && legs.type == ItemID.SpiderGreaves)
                return "Cataclysm:SpiderArmor";

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            switch (set)
            {
                case "Cataclysm:SpiderArmor":
                    if (CalamityChangesConfig.Instance.spiderArmorBuff)
                    {
                        player.setBonus += "\nYou can stick to walls like a spider";
                        player.spikedBoots = 3;
                    }
                    break;
            }
        }
    }
}