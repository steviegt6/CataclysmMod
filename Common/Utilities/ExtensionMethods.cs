using MonoMod.Cil;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Attempts to find a recipe according to the provided <see cref="RecipeFinder"/>.
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

        /// <summary>
        /// Creates an <see cref="ILCursor"/> with an <see cref="ILContext"/>.
        /// </summary>
        public static void CreateCursor(this ILContext context, out ILCursor c) => c = new ILCursor(context);
    }
}