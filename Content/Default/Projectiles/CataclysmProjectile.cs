using CataclysmMod.Core.ModCompatibility;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Default.Projectiles
{
    public class CataclysmProjectile : ModProjectile, IModDependent
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