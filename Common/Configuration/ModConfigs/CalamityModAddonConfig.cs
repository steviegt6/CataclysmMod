#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Configuration.ModConfigs
{
    [AddonContent(typeof(AutoloadAddon))]
    public class CalamityModAddonConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;
    }
}