using Terraria.ModLoader;

namespace CataclysmMod.Content.Default.Recipes
{
    public abstract class RecipeContainer // : IModDependent
    {
        public virtual void PreAddRecipes(Mod mod)
        {
        }

        public virtual void AddRecipes(Mod mod)
        {
        }

        public virtual void PostAddRecipes(Mod mod)
        {
        }

        public virtual void PreAddRecipeGroups(Mod mod)
        {
        }

        public virtual void AddRecipeGroups(Mod mod)
        {
        }

        public virtual void PostAddRecipeGroups(Mod mod)
        {
        }
    }
}