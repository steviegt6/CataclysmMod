#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;

namespace CataclysmMod.Common.Addons
{
    /// <summary>
    ///     Represents an addon that is always enabled.
    /// </summary>
    public class AutoloadAddon : Addon<AutoloadAddon>
    {
        public override string InternalName => "Terraria";

        public override string DisplayName => "Invisible";

        public override Version MinimumVersion => new Version(1, 3, 5, 1);

        public override bool IsEnabled => true;
    }
}