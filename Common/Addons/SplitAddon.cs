#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;

namespace CataclysmMod.Common.Addons
{
    public class SplitAddon : Addon<SplitAddon>
    {
        public override string InternalName => "Split";
        
        public override Version MinimumVersion => new Version(0, 4, 1, 10);
    }
}