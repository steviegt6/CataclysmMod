// Copyright (C) 2021 Tomat and Contributors, MIT License

using System;
using System.Reflection;
using CalamityMod.Prefixes;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Calamity.Items.Weapons.Rogue;
using CataclysmMod.Content.Default.MonoMod;
using Terraria;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    public class RoguePrefixApplyPatch : MonoModPatcher<MethodInfo>
    {
        public override MethodInfo Method => typeof(RoguePrefix).GetCachedMethod(nameof(RoguePrefix.Apply));

        public override MethodInfo ModderMethod => GetType().GetCachedMethod(nameof(ApplyOnFakes));

        public static void ApplyOnFakes(Action<RoguePrefix, Item> orig, RoguePrefix self, Item item)
        {
            orig(self, item);

            if (item.modItem is RogueCataclysmWeapon rogue)
                rogue.StealthStrikeDamage = self.GetFieldValue<RoguePrefix, float>("stealthDmgMult");
        }
    }
}