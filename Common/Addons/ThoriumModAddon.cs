#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;

namespace CataclysmMod.Common.Addons
{
    public class ThoriumModAddon : Addon<ThoriumModAddon>
    {
        public override string InternalName => "ThoriumMod";

        public override string DisplayName => "Thorium Mod";

        public override Version MinimumVersion => new Version(1, 6, 5, 4);
    }
}