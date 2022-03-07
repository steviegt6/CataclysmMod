#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Reflection;
using CalamityMod.Prefixes;
using CataclysmMod.Content.CalamityMod.Components.Items;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Weaving;
using Rejuvena.Backscatter.Cache;
using Terraria;

namespace CataclysmMod.Content.CalamityMod.Patches
{
    public class RoguePrefixApplyPatch : ILoadable
    {
        public void Load()
        {
            HookCreator.Detour(
                typeof(RoguePrefix).GetMethod(nameof(RoguePrefix.Apply), BindingFlags.Instance | BindingFlags.Public),
                GetType().GetMethod(nameof(ApplyOnFakes), BindingFlags.Static | BindingFlags.Public)
            );
        }
        
        public static void ApplyOnFakes(Action<RoguePrefix, Item> orig, RoguePrefix self, Item item)
        {
            orig(self, item);

            if (item.modItem is IRogueItemComponent rogue)
                rogue.StealthStrikeDamage = self.GetFieldValue<RoguePrefix, float>("stealthDmgMult");
        }
    }
}