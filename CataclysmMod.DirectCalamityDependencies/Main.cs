using CataclysmMod.Common.DirectDependencies;
using DirectCalamityDependencies;
using Terraria.ModLoader;

// It MUST be in the namespace "ROOT"
namespace ROOT
{
    public class Main : DirectDependency
    {
        public override string DependsOn => "CalamityMod";

        public override void AddContent(Mod mod)
        {
            mod.AddItem("DecreeDagger", new DecreeDagger());
            mod.AddProjectile("DecreeDaggerProj", new DecreeDaggerProj());
            mod.AddProjectile("DecreeDaggerSplitProj", new DecreeDaggerSplitProj());
        }
    }
}
