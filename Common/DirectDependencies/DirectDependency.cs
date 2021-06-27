using Terraria.ModLoader;

namespace CataclysmMod.Common.DirectDependencies
{
    public abstract class DirectDependency
    {
        public abstract string DependsOn { get; }

        public abstract void AddContent(Mod mod);
    }
}
