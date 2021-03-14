using CalamityMod.Projectiles.Summon;
using CataclysmMod.Content.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Projectiles
{
    public class FungalClumpLightEmissionProj : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            if (projectile.type != ModContent.ProjectileType<FungalClumpMinion>() || !CataclysmConfig.Instance.fungalClumpEmitsLight)
                return;

            Vector3 light = new Vector3(22f / 200f, 54f / 255f, 125f / 255f); // weird calculations man
            Lighting.AddLight(projectile.position, light);
        }
    }
}
