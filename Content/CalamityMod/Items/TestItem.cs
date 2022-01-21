#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria.ModLoader;

namespace CataclysmMod.Content.CalamityMod.Items
{
    [AddonContent(typeof(CalamityModAddon))]
    public sealed class TestItem : ModItem
    {
        public override string Texture => "Terraria/Item_1";
    }
}