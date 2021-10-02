using System.Linq;
using CataclysmMod.Content.Default.GlobalModifications;
using CataclysmMod.Content.Split.Projectiles;
using CataclysmMod.Core.ModCompatibility;
using Split.Projectiles.Hostile.Bosses.Menace;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.GlobalModifications.GlobalProjectiles
{
    [ModDependency("Split")]
    public class MenaceUmbrellaGlobalProjectile : CataclysmGlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            base.AI(projectile);

            bool MenaceRainProjectile(int type) => type == ModContent.ProjectileType<MenacingKnife>() ||
                                                   type == ModContent.ProjectileType<IceBlock>();

            foreach (Projectile proj in Main.projectile.Where(x =>
                x.active && x.type.Equals(ModContent.ProjectileType<PlayerUmbrellaProjectile>())))
                if (MenaceRainProjectile(projectile.type) && proj.Hitbox.Intersects(projectile.Hitbox))
                    projectile.Kill();
        }
    }
}