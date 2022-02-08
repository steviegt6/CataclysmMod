#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CataclysmMod.Core.Loading;

namespace CataclysmMod.Common.Addons
{
    public class CalamityModAddon : Addon<CalamityModAddon>
    {
        public override string InternalName => "CalamityMod";
        
        public override string DisplayName => "Calamity Mod";

        public override Version MinimumVersion => new Version(1, 5, 0, 3);
        
        public override string Description => "Pending...";
    }
}