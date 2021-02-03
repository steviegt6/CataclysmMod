using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Configs
{
    [Label("Thorium Changes")]
    public class ThoriumChangesConfig : ModConfig
    {
        public static ThoriumChangesConfig Instance { get; private set; }

        public override void OnLoaded() => Instance = this;

        public override ConfigScope Mode => ConfigScope.ClientSide;

        // Item Changes
        [Header("Item Changes")]
        [Label("Fragment Crafting Recipes")]
        [Tooltip("Modifies vanilla fragment recipes require 3 of any fragment to craft and gives the Thorium fragments the same recipe.")]
        [DefaultValue(true)]
        public bool fragmentRecipes;
    }
}