using System;
using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles
{
    public class DecreeDaggerProj : ModProjectile
    {
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
            projectile.penetrate = 5;
            projectile.aiStyle = 2;
            projectile.timeLeft = 600;
            projectile.Calamity().rogue = true;
            projectile.localNPCHitCooldown = 10;

            aiType = 48;
        }

        public override void AI()
        {
            if (Main.rand.NextBool(4))
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75,
                    projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

            if (projectile.Calamity().stealthStrike && projectile.timeLeft % 8 == 0 &&
                projectile.owner == Main.myPlayer)
            {
                Projectile proj = Projectile.NewProjectileDirect(projectile.Center, new Vector2(-14f, 14f),
                    Main.rand.NextBool(2) ? ProjectileID.CursedFlameFriendly : ProjectileID.CursedDartFlame,
                    (int) (projectile.damage * 0.5f), projectile.knockBack * 0.5f, projectile.owner);

                proj.Calamity().stealthStrike = true;
                proj.usesLocalNPCImmunity = true;
                proj.localNPCHitCooldown = 10;
            }

            Vector2 center = projectile.Center;
            bool doSpecial = false;

            for (int i = 0; i < 200; i++)
                if (Main.npc[i].CanBeChasedBy(projectile))
                {
                    float offset = Main.npc[i].width / 2f + Main.npc[i].height / 2f;
                    bool special = projectile.Calamity().stealthStrike ||
                                   Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1);

                    if (!(Vector2.Distance(Main.npc[i].Center, projectile.Center) < offset + offset) || !special)
                        continue;

                    center = Main.npc[i].Center;
                    doSpecial = true;
                    break;
                }

            if (!doSpecial)
                return;

            Vector2 direction = projectile.DirectionTo(center);

            projectile.extraUpdates = 1;

            if (direction.HasNaNs())
                direction = Vector2.UnitX;

            projectile.velocity = (projectile.velocity * 20f + direction * 12f) / 21f;
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 10; i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75,
                    projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);

            if (!projectile.Calamity().stealthStrike)
                return;

            for (int i = 0; i < 2; i++)
                Projectile.NewProjectile(projectile.position,
                    new Vector2(Main.rand.Next(1, 5) * (Main.rand.NextBool()
                            ? 1
                            : -1),
                        Main.rand.Next(1, 5) * (Main.rand.NextBool()
                            ? 1
                            : -1)),
                    ModContent.ProjectileType<DecreeDaggerSplitProj>(), projectile.damage, projectile.knockBack,
                    projectile.owner);
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            projectile.penetrate--;

            if (projectile.penetrate <= 0)
            {
                projectile.Kill();
            }
            else
            {
                projectile.ai[0] += 0.1f;

                if (Math.Abs(projectile.velocity.X - oldVelocity.X) > 0f)
                    projectile.velocity.X = 0f - oldVelocity.X;

                if (Math.Abs(projectile.velocity.Y - oldVelocity.Y) > 0f)
                    projectile.velocity.Y = 0f - oldVelocity.Y;
            }

            return false;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D projTex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(projTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor),
                projectile.rotation, projTex.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) =>
            target.AddBuff(39, projectile.Calamity().stealthStrike
                ? 600
                : 120);

        public override void OnHitPvp(Player target, int damage, bool crit) =>
            target.AddBuff(39, projectile.Calamity().stealthStrike
                ? 600
                : 120);
    }
}