using System;
using System.Reflection;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class ExtensionMethods
    {
        /// <summary>
        ///     Attempts to find a recipe according to the provided <see cref="RecipeFinder" />.
        /// </summary>
        /// <param name="finder"></param>
        /// <param name="editor"></param>
        /// <returns></returns>
        public static bool TryFindExactRecipe(this RecipeFinder finder, out RecipeEditor editor)
        {
            Recipe foundRecipe = finder.FindExactRecipe();

            if (foundRecipe == null)
            {
                editor = null;
                return false;
            }

            editor = new RecipeEditor(finder.FindExactRecipe());
            return true;
        }

        public static MethodInfo GetMethodForced(this Type type, string method) => type.GetMethod(method,
            BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
    }
}