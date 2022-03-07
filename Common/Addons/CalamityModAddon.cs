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
    public class CalamityModAddon : Addon<CalamityModAddon>
    {
        public override string InternalName => "CalamityMod";
        
        public override string DisplayName => "Calamity Mod";

        public override Version MinimumVersion => new Version(1, 5, 0, 4);

        public override ModConfig Config => ModContent.GetInstance<CalamityModAddonConfig>();

        public override string Description => FilelessEntries.GetCalamityDescription();
    }
}