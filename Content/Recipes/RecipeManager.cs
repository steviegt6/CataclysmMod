using System;
using System.Collections.Generic;

namespace CataclysmMod.Content.Recipes
{
    public static class RecipeManager
    {
        public static List<ModCompatRecipe> ModCompatRecipes;

        public static void Load()
        {
            ModCompatRecipes = new List<ModCompatRecipe>();

            foreach (Type type in CataclysmMod.Instance.Code.GetTypes())
                if (!type.IsAbstract && type.GetConstructor(new Type[] { }) != null && type.IsSubclassOf(typeof(ModCompatRecipe)))
                {
                    ModCompatRecipe compatRecipe = Activator.CreateInstance(type) as ModCompatRecipe;

                    if (compatRecipe.Autoload())
                        ModCompatRecipes.Add(compatRecipe);
                }

            foreach (ModCompatRecipe compatRecipe in ModCompatRecipes)
                compatRecipe.AddRecipes();
        }

        public static void ModifyRecipes()
        {
            foreach (ModCompatRecipe compatRecipe in ModCompatRecipes)
                compatRecipe.ModifyRecipes();
        }

        public static void Unload()
        {
            foreach (ModCompatRecipe compatRecipe in ModCompatRecipes)
                compatRecipe.Unload();

            ModCompatRecipes = null;
        }
    }
}