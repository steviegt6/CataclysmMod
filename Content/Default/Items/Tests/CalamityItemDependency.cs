﻿using CataclysmMod.Core.ModCompatibility;

namespace CataclysmMod.Content.Default.Items.Tests
{
    [ModDependency("CalamityMod")]
    public class CalamityItemDependency : CataclysmItem
    {
        public override bool LoadWithValidMods() => false;
    }
}