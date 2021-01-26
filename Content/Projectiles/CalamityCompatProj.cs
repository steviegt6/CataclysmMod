using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles
{
    public abstract class CalamityCompatProj : ModProjectile
    {
        public override bool Autoload(ref string name) => CataclysmMod.Instance.Calamity != null;
    }
}