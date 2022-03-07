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
    public class SplitAddon : Addon<SplitAddon>
    {
        public override string InternalName => "Split";
        
        public override string DisplayName => "Split";

        public override Version MinimumVersion => new Version(0, 4, 1, 10);
        
        public override ModConfig Config => ModContent.GetInstance<SplitAddonConfig>();
        
        public override string Description => FilelessEntries.GetSplitDescription();
    }
}