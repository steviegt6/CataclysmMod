// Copyright (C) 2021 Tomat and Contributors, MIT License

using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CataclysmMod.Content.Default.Items;
using CataclysmMod.Core.ModCompatibility;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CataclysmMod.Content.Calamity.Items.Weapons.Rogue
{
    [ModDependency("CalamityMod")]
    public abstract class RogueCataclysmWeapon : CataclysmItem
    {
        public float StealthStrikeDamage;

        public override ModItem Clone(Item itemClone)
        {
            RogueCataclysmWeapon weapon = (RogueCataclysmWeapon) base.Clone(itemClone);
            weapon.StealthStrikeDamage = StealthStrikeDamage;
            return weapon;
        }

        public override int ChoosePrefix(UnifiedRandom rand)
        {
            WeightedRandom<string> prefix = new WeightedRandom<string>();
            prefix.Add("Pointy");
            prefix.Add("Sharp");
            prefix.Add("Feathered");
            prefix.Add("Sleek");
            prefix.Add("Hefty");
            prefix.Add("Mighty");
            prefix.Add("Glorious");
            prefix.Add("Serrated");
            prefix.Add("Vicious");
            prefix.Add("Lethal");
            prefix.Add("Flawless");
            prefix.Add("Radical");
            prefix.Add("Blunt");
            prefix.Add("Flimsy");
            prefix.Add("Unbalanced");
            prefix.Add("Atrocious");

            return ModLoader.GetMod("CalamityMod").GetPrefix(prefix.Get()).Type;
        }

        public override bool NewPreReforge()
        {
            StealthStrikeDamage = 1f;
            return true;
        }

        public override bool? PrefixChance(int pre, UnifiedRandom rand)
        {
            if (item.maxStack > 1)
                return false;

            return null;
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.melee = item.ranged = item.magic = item.summon = false;
            item.thrown = true;
        }

        public override void ModifyWeaponDamage(Player player, ref float add, ref float mult, ref float flat)
        {
            add += player.Calamity().throwingDamage - 1f;

            if (player.Calamity().StealthStrikeAvailable() && item.prefix > 0)
                mult += StealthStrikeDamage - 1f;
        }

        public override void GetWeaponCrit(Player player, ref int crit) => crit += player.Calamity().throwingCrit;

        public override float UseTimeMultiplier(Player player)
        {
            float mult = 1f;

            if (player.Calamity().gloveOfPrecision)
                mult -= 0.2f;

            if (player.Calamity().gloveOfRecklessness)
                mult += 0.2f;

            if (player.Calamity().titanHeartMantle)
                mult -= 0.15f;

            return mult;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            TooltipLine damageLine = tooltips.FirstOrDefault(x => x.Name == "Damage" && x.mod == "Terraria");

            if (damageLine != null)
            {
                string[] source = damageLine.text.Split(' ');
                string first = source.First();
                string last = source.Last();
                damageLine.text = first + " rogue " + last;
            }

            if (item.prefix <= 0)
                return;

            float stealthDamage = StealthStrikeDamage - 1f;
            if (stealthDamage > 0f)
            {
                TooltipLine line = new TooltipLine(mod, "PrefixSSDmg",
                    "+" + Math.Round(stealthDamage * 100f) + "% stealth strike damage") {isModifier = true};
                tooltips.Add(line);
            }
            else if (stealthDamage < 0f)
            {
                TooltipLine line = new TooltipLine(mod, "PrefixSSDmg",
                        "-" + Math.Round(Math.Abs(stealthDamage) * 100f) + "% stealth strike damage")
                    {isModifier = true, isModifierBad = true};
                tooltips.Add(line);
            }
        }

        public override bool ConsumeAmmo(Player player) => Main.rand.NextFloat() <= player.Calamity().throwingAmmoCost;
    }
}