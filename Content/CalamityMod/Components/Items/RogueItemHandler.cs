#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using CalamityMod;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace CataclysmMod.Content.CalamityMod.Components.Items
{
    [AddonContent(typeof(CalamityModAddon))]
    public class RogueItemHandler : GlobalItem
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public override int ChoosePrefix(Item item, UnifiedRandom rand)
        {
            if (!(item.modItem is IRogueItemComponent))
                return base.ChoosePrefix(item, rand);
            
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
        
        public override bool NewPreReforge(Item item)
        {
            if (!(item.modItem is IRogueItemComponent rogueItem))
                return base.NewPreReforge(item);
            
            rogueItem.StealthStrikeDamage = 1f;
            return true;
        }

        public override bool? PrefixChance(Item item, int pre, UnifiedRandom rand)
        {
            if (!(item.modItem is IRogueItemComponent))
                return base.PrefixChance(item, pre, rand);
            
            if (item.maxStack > 1)
                return false;

            return null;
        }

        public override void SetDefaults(Item item)
        {
            if (!(item.modItem is IRogueItemComponent))
                return;

            item.melee = item.ranged = item.magic = item.summon = false;
            item.thrown = true;
        }

        public override void ModifyWeaponDamage(Item item, Player player, ref float add, ref float mult, ref float flat)
        {
            if (!(item.modItem is IRogueItemComponent rogueItem))
                return;
            
            add += player.Calamity().throwingDamage - 1f;

            if (player.Calamity().StealthStrikeAvailable() && item.prefix > 0)
                mult += rogueItem.StealthStrikeDamage - 1f;
        }

        public override void GetWeaponCrit(Item item, Player player, ref int crit)
        {
            if (!(item.modItem is IRogueItemComponent))
                return;
            
            crit += player.Calamity().throwingCrit;
        }

        public override float UseTimeMultiplier(Item item, Player player)
        {
            if (!(item.modItem is IRogueItemComponent))
                return base.UseTimeMultiplier(item, player);
            
            float mult = 1f;

            if (player.Calamity().gloveOfPrecision)
                mult -= 0.2f;

            if (player.Calamity().gloveOfRecklessness)
                mult += 0.2f;

            if (player.Calamity().titanHeartMantle)
                mult -= 0.15f;

            return mult;
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (!(item.modItem is IRogueItemComponent rogueItem))
                return;
            
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

            float stealthDamage = rogueItem.StealthStrikeDamage - 1f;
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

        public override bool ConsumeAmmo(Item item, Player player)
        {
            if (!(item.modItem is IRogueItemComponent rogueItem))
                return base.ConsumeAmmo(item, player);
            
            return Main.rand.NextFloat() <= player.Calamity().throwingAmmoCost;
        }
    }
}