using System.Collections.Generic;
using Terraria.ModLoader;

namespace CataclysmMod.DirectCalamityDependencies
{
    public class Main
    {
        public IEnumerable<string> DependsOn
        {
            get { yield return "CalamityMod"; }
        }

        public void AddContent(Mod mod)
        {
            mod.AddItem("DecreeDagger", new DecreeDagger());
            mod.AddProjectile("DecreeDaggerProj", new DecreeDaggerProj());
            mod.AddProjectile("DecreeDaggerSplitProj", new DecreeDaggerSplitProj());
        }
    }
}