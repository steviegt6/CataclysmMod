#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Common.Configuration.ModConfigs;
using CataclysmMod.Core.Loading;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Addons
{
    public class ThoriumModAddon : Addon<ThoriumModAddon>
    {
        public override string InternalName => "ThoriumMod";

        public override string DisplayName => "Thorium Mod";

        public override Version MinimumVersion => new Version(1, 6, 5, 4);
        
        public override ModConfig Config => ModContent.GetInstance<ThoriumModAddonConfig>();

        public override string Description => "Pending...";
    }
}