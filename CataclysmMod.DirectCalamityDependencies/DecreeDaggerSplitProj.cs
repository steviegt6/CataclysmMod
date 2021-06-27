using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DirectCalamityDependencies
{
    public class DecreeDaggerSplitProj : ModProjectile
    {
        public int SplitTime;

        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CursedDagger";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Dagger");

            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = projectile.height = 12;
            projectile.friendly = true;
            projectile.aiStyle = 2;
            projectile.timeLeft = 600;
            projectile.localNPCHitCooldown = 10;
            aiType = 48;
            projectile.Calamity().rogue = true;
        }

        public override void AI()
        {
            SplitTime++;

            if (Terraria.Main.rand.NextBool(4))
            {
                Vector2 spawnPos = projectile.position + projectile.velocity;
                Vector2 spawnSpeed = new Vector2(projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

                Dust.NewDust(spawnPos, projectile.width, projectile.height, DustID.CursedTorch, spawnSpeed.X, spawnSpeed.Y);
            }

            if (projectile.Calamity().stealthStrike && projectile.timeLeft % 8 == 0 &&
                projectile.owner == Terraria.Main.myPlayer)
            {
                Vector2 velocity = new Vector2(-14f, 14f);
                int type = Terraria.Main.rand.NextBool(2)
                    ? ProjectileID.CursedFlameFriendly
                    : ProjectileID.CursedDartFlame;

                Projectile stealthStrikeExtra = Projectile.NewProjectileDirect(projectile.Center, velocity, type,
                    (int) (projectile.damage * 0.5f), projectile.knockBack * 0.5f, projectile.owner);
                stealthStrikeExtra.usesLocalNPCImmunity = true;
                stealthStrikeExtra.localNPCHitCooldown = 10;
                projectile.Calamity().forceRogue = true;
            }

            Vector2 center = projectile.Center;
            bool doSpecial = false;

            foreach (NPC npc in Terraria.Main.npc)
                if (npc.CanBeChasedBy(projectile) && SplitTime >= 120)
                {
                    float offset = npc.width / 2f + npc.height / 2f;
                    bool special = projectile.Calamity().stealthStrike ||
                                   Collision.CanHit(projectile.Center, 1, 1, npc.Center, 1, 1);

                    if (!(Vector2.Distance(npc.Center, projectile.Center) < offset + offset) || !special)
                        continue;

                    center = npc.Center;
                    doSpecial = true;
                    break;
                }

            if (!doSpecial)
                return;

            projectile.extraUpdates = 1;
            Vector2 direction = projectile.DirectionTo(center);

            if (direction.HasNaNs())
                direction = Vector2.UnitX;

            Vector2 baseVelocity = projectile.velocity * 20f + direction * 12f;
            projectile.velocity = baseVelocity / 21f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector2 spawnPos = projectile.position + projectile.velocity;
                Vector2 spawnSpeed = new Vector2(projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);

                Dust.NewDust(spawnPos, projectile.width, projectile.height, DustID.CursedTorch, spawnSpeed.X, spawnSpeed.Y);
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D projTex = Terraria.Main.projectileTexture[projectile.type];
            Vector2 drawPos = projectile.Center - Terraria.Main.screenPosition;
            Vector2 drawOrigin = projTex.Size() / 2f;

            spriteBatch.Draw(projTex, drawPos, null, projectile.GetAlpha(lightColor), projectile.rotation, drawOrigin,
                projectile.scale, SpriteEffects.None, 0f);
            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) =>
            target.AddBuff(39, projectile.Calamity().stealthStrike ? 600 : 120);

        public override void OnHitPvp(Player target, int damage, bool crit) =>
            target.AddBuff(39, projectile.Calamity().stealthStrike ? 600 : 120);
    }
}