#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Common.Configuration.ModConfigs;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Addons
{
    public class ClickerClassAddon : Addon<ClickerClassAddon>
    {
        public override string InternalName => "ClickerClass";
        
        public override string DisplayName => "The Clicker Class";

        public override Version MinimumVersion => new Version(1, 2, 7, 1);

        public override ModConfig Config => ModContent.GetInstance<ClickerClassAddonConfig>();

        public override string Description => FilelessEntries.GetClickerClassDescription();
    }
}