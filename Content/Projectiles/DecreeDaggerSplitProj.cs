using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles
{
    public class DecreeDaggerSplitProj : ModProjectile
    {
        public int splitTime = 0;

        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CursedDagger";

        public override bool Autoload(ref string name) => false;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cursed Dagger");

            ProjectileID.Sets.TrailCacheLength[projectile.type] = 3;
            ProjectileID.Sets.TrailingMode[projectile.type] = 0;
        }

        public override void SetDefaults()
        {
            projectile.width = 12;
            projectile.height = 12;

            projectile.friendly = true;
            projectile.aiStyle = 2;
            projectile.timeLeft = 600;
            projectile.Calamity().rogue = true;
            projectile.localNPCHitCooldown = 10;
            aiType = 48;
        }

        public override void AI()
        {
            splitTime++;

            if (Main.rand.NextBool(4))
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.velocity.X * 0.5f, projectile.velocity.Y * 0.5f);

            if (projectile.Calamity().stealthStrike && projectile.timeLeft % 8 == 0 && projectile.owner == Main.myPlayer)
            {
                int weakerProj = Projectile.NewProjectile(projectile.Center, new Vector2(-14f, 14f), Main.rand.NextBool(2) ? ProjectileID.CursedFlameFriendly : ProjectileID.CursedDartFlame, (int)(projectile.damage * 0.5f), projectile.knockBack * 0.5f, projectile.owner);
                Projectile proj = Main.projectile[weakerProj];

                proj.Calamity().forceRogue = true;
                proj.usesLocalNPCImmunity = true;
                proj.localNPCHitCooldown = 10;
            }

            Vector2 center = projectile.Center;
            bool doSpecial = false;

            for (int i = 0; i < 200; i++)
                if (Main.npc[i].CanBeChasedBy(projectile) && splitTime >= 120)
                {
                    float offset = (Main.npc[i].width / 2f) + (Main.npc[i].height / 2f);
                    bool special = projectile.Calamity().stealthStrike || Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1);

                    if (Vector2.Distance(Main.npc[i].Center, projectile.Center) < offset + offset && special)
                    {
                        center = Main.npc[i].Center;
                        doSpecial = true;
                        break;
                    }
                }

            if (doSpecial)
            {
                projectile.extraUpdates = 1;
                Vector2 direction = projectile.DirectionTo(center);

                if (direction.HasNaNs())
                    direction = Vector2.UnitX;

                projectile.velocity = (projectile.velocity * 20f + direction * 12f) / 21f;
            }
        }

        public override void Kill(int timeLeft)
        {
            for (int i = 0; i < 20; i++)
                Dust.NewDust(projectile.position + projectile.velocity, projectile.width, projectile.height, 75, projectile.oldVelocity.X * 0.5f, projectile.oldVelocity.Y * 0.5f);
        }

        public override bool OnTileCollide(Vector2 oldVelocity) => false;

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor)
        {
            Texture2D projTex = Main.projectileTexture[projectile.type];
            spriteBatch.Draw(projTex, projectile.Center - Main.screenPosition, null, projectile.GetAlpha(lightColor), projectile.rotation, projTex.Size() / 2f, projectile.scale, SpriteEffects.None, 0f);

            return false;
        }

        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit) => target.AddBuff(39, projectile.Calamity().stealthStrike ? 600 : 120);

        public override void OnHitPvp(Player target, int damage, bool crit) => target.AddBuff(39, projectile.Calamity().stealthStrike ? 600 : 120);
    }
}