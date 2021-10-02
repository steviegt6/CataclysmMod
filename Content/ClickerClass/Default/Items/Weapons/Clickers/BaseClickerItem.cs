﻿using CataclysmMod.Content.Default.Items;
using CataclysmMod.Core.ModCompatibility;
using Microsoft.Xna.Framework;

namespace CataclysmMod.Content.ClickerClass.Default.Items.Weapons.Clickers
{
    [ModDependency("ClickerClass")]
    public abstract class BaseClickerItem : CataclysmItem
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