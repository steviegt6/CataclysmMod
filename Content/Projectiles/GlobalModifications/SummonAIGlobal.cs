using CalamityMod;
using CalamityMod.Buffs.Summon;
using CalamityMod.CalPlayer;
using CalamityMod.Projectiles;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class SummonAIGlobal : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public int iceClasperMinionDust = 3;

        public static List<int> SummonAIOverhauls = new List<int>()
        {
            ModContent.ProjectileType<IceClasperMinion>()
        };

        public override bool PreAI(Projectile projectile) => !HasCustomAI(projectile);

        public override void PostAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<IceClasperMinion>())
                IceClasperMinionAI(projectile);
        }

        public void IceClasperMinionAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];
            CalamityPlayer calPlayer = player.Calamity();
            CalamityGlobalProjectile calProj = projectile.Calamity();

            // Spawn a "circle" of dust on spawn
            if (iceClasperMinionDust > 0)
            {
                calProj.spawnedPlayerMinionDamageValue = player.MinionDamage();
                calProj.spawnedPlayerMinionProjectileDamageValue = projectile.damage;

                for (int i = 0; i < 36; i++)
                {
                    Vector2 normalizedVelocity = Vector2.Normalize(projectile.velocity);
                    Vector2 adjustedVelocity = normalizedVelocity * new Vector2(projectile.width / 2f, projectile.height) * 0.75f;
                    Vector2 rotation = adjustedVelocity.RotatedBy((i - (36 / 2 - 1)) * ((float)Math.PI * 2f) / 36) + projectile.Center;
                    Vector2 correctedCenter = rotation - projectile.Center;

                    // Dust ID 67: Unknown.
                    // There's no field for it in DustID.cs
                    // Unfortunate.
                    Dust dust = Dust.NewDustDirect(rotation + correctedCenter, 0, 0, 67, correctedCenter.X * 1.75f, correctedCenter.Y * 1.75f, 100, Scale: 1.1f);
                    dust.noGravity = true;
                    dust.velocity = correctedCenter;
                }

                iceClasperMinionDust--;
            }

            // Correct damage in case it changes after the minion was spawned
            if (player.MinionDamage() != calProj.spawnedPlayerMinionDamageValue)
            {
                int correctedDamage = (int)(calProj.spawnedPlayerMinionProjectileDamageValue / calProj.spawnedPlayerMinionDamageValue * player.MinionDamage());
                projectile.damage = correctedDamage;
            }

            // Handle buff clearing
            player.AddBuff(ModContent.BuffType<IceClasper>(), 60 * 60);

            if (projectile.type == ModContent.ProjectileType<IceClasperMinion>())
            {
                if (player.dead)
                    calPlayer.iClasper = false;

                if (calPlayer.iClasper)
                    projectile.timeLeft = 2;
            }

            // Calamity extension-method that handles charging AI automatially
            projectile.ChargingMinionAI(1200f, 1500f, 2200f, 150f, 0, 40f, 9f, 4f, new Vector2(0f, -60f), 40f, 9f, true, true);

            /* Visuals */

            // Glow
            Lighting.AddLight(projectile.Center / 16f, new Vector3(0.05f, 0.15f, 0.2f));

            // Frame animations
            projectile.frameCounter++;

            if (projectile.frameCounter > 6)
            {
                projectile.frame++;
                projectile.frameCounter = 0;
            }

            if (projectile.frame > 5)
                projectile.frame = 0;

            LerpToRotation(projectile);
        }

        public static bool HasCustomAI(Projectile projectile) => SummonAIOverhauls.Contains(projectile.type);

        public static void LerpToRotation(Projectile projectile, float amount = 0.15f) => projectile.rotation = projectile.rotation.AngleTowards(projectile.velocity.ToRotation() - MathHelper.PiOver2, amount);
    }
}