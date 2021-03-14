using CalamityMod.Projectiles.Ranged;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Projectiles
{
    public class MakeDrataliornusGoodProj : GlobalProjectile
    {
        public override void PostAI(Projectile projectile)
        {
            if (CataclysmConfig.Instance.drataliornusArrowsThroughBlocks &&
                projectile.type == ModContent.ProjectileType<DrataliornusFlame>())
                projectile.tileCollide = false;
        }
    }
}