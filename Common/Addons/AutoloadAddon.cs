#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;
using Terraria;

namespace CataclysmMod.Common.Addons
{
    /// <summary>
    ///     Represents an addon that is always enabled.
    /// </summary>
    public class AutoloadAddon : Addon<AutoloadAddon>
    {
        public override string InternalName => "Terraria";
        
        public override Version MinimumVersion => Version.Parse(Main.versionNumber);

        public override bool IsEnabled => true;
    }
}