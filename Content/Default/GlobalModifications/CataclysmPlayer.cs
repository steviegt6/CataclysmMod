using CataclysmMod.Common.ModCompatibility;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Default.GlobalModifications
{
    public abstract class CataclysmPlayer : ModPlayer, IModDependent
    {
        public virtual bool LoadWithValidMods() => true;

        public virtual bool DependsOnMod() => true;

        public sealed override bool Autoload(ref string name)
        {
            ModifyContentName(ref name);
            return !DependsOnMod();
        }

        public virtual void ModifyContentName(ref string name)
        {
        }
    }
}