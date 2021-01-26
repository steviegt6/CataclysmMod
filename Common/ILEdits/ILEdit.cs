namespace CataclysmMod.Common.ILEdits
{
    public abstract class ILEdit
    {
        public abstract string DictKey { get; }

        public virtual bool Autoload() => true;

        public abstract void Load();

        public abstract void Unload();
    }
}