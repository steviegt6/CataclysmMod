using CalamityMod;
using CalamityMod.Projectiles.Summon;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class FungalClumpProjGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public int defDamage = 0;

        public bool firstFrame = true;

        public override void AI(Projectile projectile)
        {
            if (CalamityChangesConfig.Instance.fungalClumpTrueDamage && projectile.type == ModContent.ProjectileType<FungalClumpMinion>())
            {
                if (firstFrame)
                {
                    defDamage = projectile.damage;
                    firstFrame = false;
                }

                Player player = Main.player[projectile.owner];

                float damageIncrease = 5f * (player.allDamage - 1f);
                damageIncrease += player.meleeDamage - 1f;
                damageIncrease += player.rangedDamage - 1f;
                damageIncrease += player.magicDamage - 1f;
                damageIncrease += player.minionDamage - 1f;
                damageIncrease += player.Calamity().throwingDamage - 1f;

                projectile.damage = (int)(damageIncrease + defDamage);
            }
        }
    }
}