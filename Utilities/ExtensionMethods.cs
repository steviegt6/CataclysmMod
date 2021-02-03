using System;
using Terraria.ModLoader;

namespace CataclysmMod.Utilities
{
    public static class ExtensionMethods
    {
        public static bool TryFindExactRecipe(this RecipeFinder finder, out RecipeEditor editor) => (editor = new RecipeEditor(finder.FindExactRecipe())) != null;

        public static bool IsNotMMHookAsm(this ResolveEventArgs args) => !args.Name.Contains("MMHOOK");
    }
}