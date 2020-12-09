using CalamityMod.Projectiles.Ranged;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class DrataliornusArrowGlobal : GlobalProjectile
    {
        public override void PostAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<DrataliornusFlame>())
                projectile.tileCollide = false;
        }
    }
}
