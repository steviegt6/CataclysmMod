using CataclysmMod.Common;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public CataclysmMod()
        {
            Instance = this;

            Properties = new ModProperties
            {
                Autoload = true,
                AutoloadBackgrounds = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public static CataclysmMod Instance { get; private set; }

        public override void Load() => ILManager.Load();

        public override void Unload() => ILManager.Unload();

        public override void AddRecipes() => RecipeHandler.AddRecipes();

        public override void PostAddRecipes() => RecipeHandler.ModifyRecipes();

        public override void AddRecipeGroups() => RecipeHandler.AddRecipeGroups();
    }
}