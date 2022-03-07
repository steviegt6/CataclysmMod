#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace CataclysmMod.Content.ClickerClass.Items.Weapons.Clickers
{
    [AddonContent(typeof(ClickerClassAddon))]
    public abstract class BaseClickerItem : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ClickerCompatibilityCalls.RegisterClickerWeapon(this);
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            ClickerCompatibilityCalls.SetClickerWeaponDefaults(item);
            item.Size = new Vector2(30f);
        }
    }
}