using System.Collections.Generic;
using Terraria.ModLoader;

namespace CataclysmMod.Common.DirectDependencies
{
    public abstract class DirectDependency
    {
        public abstract IEnumerable<string> DependsOn { get; }

        public abstract void AddContent(Mod mod);
    }
}
