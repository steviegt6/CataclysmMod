using Terraria.ModLoader;

namespace CataclysmMod.DirectCalamityDependencies
{
    public static class CalamityDependencyContent
    {
        public static void AddContent(Mod mod)
        {
            mod.AddItem("DecreeDagger", new DecreeDagger());
            mod.AddProjectile("DecreeDaggerProj", new DecreeDaggerProj());
            mod.AddProjectile("DecreeDaggerSplitProj", new DecreeDaggerSplitProj());
        }
    }
}
