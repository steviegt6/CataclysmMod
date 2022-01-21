#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;

namespace CataclysmMod.Common.Addons
{
    public class ClickerClassAddon : Addon<ClickerClassAddon>
    {
        public override string InternalName => "ClickerClass";
        
        public override Version MinimumVersion => new Version(1, 2, 7, 1);
    }
}