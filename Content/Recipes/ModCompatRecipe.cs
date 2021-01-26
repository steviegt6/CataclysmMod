namespace CataclysmMod.Content.Recipes
{
    public abstract class ModCompatRecipe
    {
        public virtual bool Autoload() => true;

        public virtual void AddRecipes()
        {
        }

        public virtual void ModifyRecipes()
        {
        }

        public virtual void Unload()
        {
        }
    }
}