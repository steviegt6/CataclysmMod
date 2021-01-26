namespace CataclysmMod.Content.Recipes
{
    public abstract class ModCompatRecipe
    {
        public virtual bool Autoload() => true;

        public virtual void Load()
        {
        }

        public virtual void Unload()
        {
        }
    }
}