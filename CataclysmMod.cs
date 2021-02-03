using CataclysmMod.Common;
using CataclysmMod.Content.Recipes;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public static CataclysmMod Instance { get; private set; }

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

        public override void Load()
        {
            ILManager.Load();
            RecipeManager.Load();
        }

        public override void Unload()
        {
            ILManager.Unload();
            RecipeManager.Unload();
        }

        public override void AddRecipes() => RecipeManager.AddRecipes();

        public override void PostAddRecipes() => RecipeManager.ModifyRecipes();

        public override void AddRecipeGroups() => RecipeManager.AddRecipeGroups();
    }
}