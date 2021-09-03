using System.Collections.Generic;
using CataclysmMod.Common.DirectDependencies;
using Terraria.ModLoader;

namespace DirectCalamityDependencies
{
    public class Load : DirectDependency
    {
        public override IEnumerable<string> DependsOn
        {
            get { yield return "CalamityMod"; }
        }

        public override void AddContent(Mod mod)
        {
            mod.AddItem("DecreeDagger", new DecreeDagger());
            mod.AddProjectile("DecreeDaggerProj", new DecreeDaggerProj());
            mod.AddProjectile("DecreeDaggerSplitProj", new DecreeDaggerSplitProj());
        }
    }
}