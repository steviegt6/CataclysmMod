using CataclysmMod.Common.ModCompatibility;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items
{
    public abstract class CataclysmItem : ModItem, IModDependent
    {
        public virtual bool LoadWithValidMods() => true;

        public virtual bool DependsOnMod() => true;

        public sealed override bool Autoload(ref string name)
        {
            ModifyContentName(ref name);
            return DependsOnMod();
        }

        public virtual void ModifyContentName(ref string name)
        {
        }
    }
}