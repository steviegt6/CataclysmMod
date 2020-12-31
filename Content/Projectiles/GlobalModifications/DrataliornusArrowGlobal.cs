using CalamityMod.Projectiles.Ranged;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class DrataliornusArrowGlobal : GlobalProjectile
    {
        public override void PostAI(Projectile projectile)
        {
            if (CalamityChangesConfig.Instance.drataliornusArrowsThroughBlocks && projectile.type == ModContent.ProjectileType<DrataliornusFlame>())
                projectile.tileCollide = false;
        }
    }
}