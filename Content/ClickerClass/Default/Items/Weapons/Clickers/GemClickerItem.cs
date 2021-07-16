using CataclysmMod.Common.ModCompatibility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.ClickerClass.Default.Items.Weapons.Clickers
{
    [ModDependency("ClickerClass")]
    public abstract class GemClickerItem : BaseClickerItem
    {
        public abstract int DamageIncrease { get; }

        public abstract float RadiusIncrease { get; }

        public abstract int GemItem { get; }

        public abstract int DustType { get; }

        public abstract Color GemColor { get; }

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

    [ModDependency("ClickerClass")]
    public class AmethystGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 0;

        public override float RadiusIncrease => 0.2f;

        public override int GemItem => ItemID.Amethyst;

        public override int DustType => 86;

        public override Color GemColor => Color.Purple;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Amethyst Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class TopazGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 0;

        public override float RadiusIncrease => 0.2f;

        public override int GemItem => ItemID.Topaz;

        public override int DustType => 87;

        public override Color GemColor => Color.Orange;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Topaz Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class SapphireGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 1;

        public override float RadiusIncrease => 0.3f;

        public override int GemItem => ItemID.Sapphire;

        public override int DustType => 88;

        public override Color GemColor => Color.Blue;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Sapphire Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class EmeraldGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 1;

        public override float RadiusIncrease => 0.3f;

        public override int GemItem => ItemID.Emerald;

        public override int DustType => 89;

        public override Color GemColor => Color.Green;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Emerald Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class RubyGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 2;

        public override float RadiusIncrease => 0.45f;

        public override int GemItem => ItemID.Topaz;

        public override int DustType => 90;

        public override Color GemColor => Color.Red;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Ruby Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class DiamondGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 2;

        public override float RadiusIncrease => 0.45f;

        public override int GemItem => ItemID.Diamond;

        public override int DustType => 91;

        public override Color GemColor => Color.LightBlue;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Diamond Clicker");
        }
    }

    [ModDependency("ClickerClass")]
    public class AmberGemClicker : GemClickerItem
    {
        public override int DamageIncrease => 3;

        public override float RadiusIncrease => 0.6f;

        public override int GemItem => ItemID.Amber;

        public override int DustType => DustID.AmberBolt;

        public override Color GemColor => Color.Goldenrod;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Amber Clicker");
        }
    }
}