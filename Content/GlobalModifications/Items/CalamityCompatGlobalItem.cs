using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CalamityMod.Items.Fishing.SulphurCatches;
using CalamityMod.Items.Potions;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.GlobalModifications.Players;
using CataclysmMod.Content.Items;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

// TODO: Split into separate files in its own folder?
namespace CataclysmMod.Content.GlobalModifications
{
    public class CalamityCompatGlobalItem : GlobalItem
    {
        public override void RightClick(Item item, Player player)
        {
            if (CataclysmConfig.Instance.sulphurousShell && item.type == ModContent.ItemType<AbyssalCrate>())
                DropHelper.DropItemChance(player, ModContent.ItemType<SulphurousShell>(), 0.1f, 1, 1);
        }

        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.SpiderMask:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 3;
                    break;

                case ItemID.SpiderBreastplate:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 2;
                    break;

                case ItemID.SpiderGreaves:
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                        item.defense += 1;
                    break;

                case ItemID.GuideVoodooDoll:
                case ItemID.ClothierVoodooDoll:
                    if (item.maxStack < 20 && CataclysmConfig.Instance.voodooDollStackIncrease)
                        item.maxStack = 20;
                    break;
            }

            if (CataclysmConfig.Instance.basherScale && item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;

            if (CataclysmConfig.Instance.sulphurSkinPotionPriceNerf &&
                item.type == ModContent.ItemType<SulphurskinPotion>())
                item.value = Item.sellPrice(silver: 2);

            if (CataclysmConfig.Instance.infinityDontConsumeAmmo && item.type == ModContent.ItemType<Infinity>())
                item.damage = 25;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            TooltipLine tooltip = tooltips.FirstOrDefault(x => x.Name == "Tooltip0" && x.mod == "Terraria");

            if (CataclysmConfig.Instance.pickaxeTooltips)
            {
                if (tooltip != null)
                {
                    if (item.type == ItemID.DeathbringerPickaxe || item.type == ItemID.NightmarePickaxe)
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineAerialite");

                    if (item.type == ItemID.Picksaw)
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineAstral");

                    if (item.type == ModContent.ItemType<FlamebeakHampick>())
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineScoriaAstral");

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe ||
                        item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe ||
                        item.type == ModContent.ItemType<GallantPickaxe>())
                        tooltip.text += "\n" + LangUtils.GetCataclysmTextValue("Tooltips.MineExodium");
                }
                else
                {
                    if (item.type == ItemID.GoldPickaxe || item.type == ItemID.PlatinumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickSeaPrism",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineSeaPrism")));

                    if (item.type == ItemID.AdamantitePickaxe || item.type == ItemID.TitaniumPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickCryonicCharred",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineCryonicCharred")));

                    if (item.type == ItemID.PickaxeAxe || item.type == ItemID.Drax ||
                        item.type == ItemID.ChlorophytePickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickPerennial",
                            LangUtils.GetCataclysmTextValue("Tooltips.Perennial")));

                    if (item.type == ItemID.SolarFlarePickaxe || item.type == ItemID.VortexPickaxe ||
                        item.type == ItemID.NebulaPickaxe || item.type == ItemID.StardustPickaxe)
                        tooltips.Add(new TooltipLine(mod, $"{mod.Name}:PickExodium",
                            LangUtils.GetCataclysmTextValue("Tooltips.MineExodium")));
                }
            }
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            if (head.type == ItemID.SpiderMask && body.type == ItemID.SpiderBreastplate &&
                legs.type == ItemID.SpiderGreaves)
                return "Cataclysm:SpiderArmor";

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            switch (set)
            {
                case "Cataclysm:SpiderArmor":
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                    {
                        player.setBonus += "\nYou can stick to walls like a spider";
                        player.spikedBoots = 3;
                    }

                    break;
            }
        }

        public override void UpdateAccessory(Item item, Player player, bool hideVisual)
        {
            switch (item.type)
            {
                case ItemID.ObsidianHorseshoe:
                case ItemID.ObsidianShield:
                case ItemID.AnkhShield:
                case ItemID.ObsidianWaterWalkingBoots:
                case ItemID.LavaWaders:
                case ItemID.ObsidianSkull:
                    player.GetModPlayer<CalamityCompatPlayer>().playerHasObsidianSkullOrTree = true;
                    break;
            }
        }

        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (item.type == ModContent.ItemType<Infinity>() && CataclysmConfig.Instance.infinityDontConsumeAmmo)
                return false;

            return base.ConsumeAmmo(item, player);
        }
    }
}