#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.ClickerClass.Items.Weapons.Clickers.Gem
{
    public abstract class GemClickerItem : BaseClickerItem
    {
        protected abstract int DamageIncrease { get; }

        protected abstract float RadiusIncrease { get; }

        protected abstract int GemItem { get; }

        protected abstract int DustType { get; }

        protected abstract Color GemColor { get; }

        public override void SetDefaults()
        {
            base.SetDefaults();

            ClickerCompatibilityCalls.SetRadius(item, 1f + RadiusIncrease);
            ClickerCompatibilityCalls.SetColor(item, GemColor);
            ClickerCompatibilityCalls.SetDust(item, DustType);
            ClickerCompatibilityCalls.AddEffect(item, "ClickerClass:DoubleClick");

            item.damage = 5 + DamageIncrease;
            item.knockBack = 1.25f;
            item.rare = ItemRarityID.White;

            Item gem = new Item();
            gem.SetDefaults(GemItem, true);
            item.value = gem.value * 3;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(GemItem, 8);
            recipe.AddTile(TileID.Anvils);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
    
    public class AmethystGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 0;

        protected override float RadiusIncrease => 0.2f;

        protected override int GemItem => ItemID.Amethyst;

        protected override int DustType => 86;

        protected override Color GemColor => Color.Purple;
    }
    
    public class TopazGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 0;

        protected override float RadiusIncrease => 0.2f;

        protected override int GemItem => ItemID.Topaz;

        protected override int DustType => 87;

        protected override Color GemColor => Color.Orange;
    }
    
    public class SapphireGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 1;

        protected override float RadiusIncrease => 0.3f;

        protected override int GemItem => ItemID.Sapphire;

        protected override int DustType => 88;

        protected override Color GemColor => Color.Blue;
    }
    
    public class EmeraldGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 1;

        protected override float RadiusIncrease => 0.3f;

        protected override int GemItem => ItemID.Emerald;

        protected override int DustType => 89;

        protected override Color GemColor => Color.Green;
    }

    public class RubyGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 2;

        protected override float RadiusIncrease => 0.45f;

        protected override int GemItem => ItemID.Ruby;

        protected override int DustType => 90;

        protected override Color GemColor => Color.Red;
    }
    
    public class DiamondGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 2;

        protected override float RadiusIncrease => 0.45f;

        protected override int GemItem => ItemID.Diamond;

        protected override int DustType => 91;

        protected override Color GemColor => Color.LightBlue;
    }
    
    public class AmberGemClicker : GemClickerItem
    {
        protected override int DamageIncrease => 3;

        protected override float RadiusIncrease => 0.6f;

        protected override int GemItem => ItemID.Amber;

        protected override int DustType => DustID.AmberBolt;

        protected override Color GemColor => Color.Goldenrod;
    }
}