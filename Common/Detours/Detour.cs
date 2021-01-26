namespace CataclysmMod.Common.Detours
{
    public abstract class Detour
    {
        public abstract string DictKey { get; }

        public virtual bool Autoload() => true;

        public abstract void Load();

        public abstract void Unload();
    }
}