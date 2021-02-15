using System.Collections.Generic;
using CalamityMod;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CataclysmMod.Content.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class CalamityCompatGlobalProjectile : GlobalProjectile
    {
        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public int defDamage;
        public bool firstFrame = true;
        public float savedRotation;

        public static Dictionary<int, float> SummonRotationAdjustments;

        public override bool PreAI(Projectile projectile)
        {
            SummonRotationAdjustments = new Dictionary<int, float>
            {
                { ModContent.ProjectileType<IceClasperMinion>(), 0.15f },
                { ModContent.ProjectileType<PowerfulRaven>(), 0.25f },
                { ModContent.ProjectileType<HerringMinion>(), 0.25f }
            };

            if (CataclysmConfig.Instance.smootherMinionRotation)
            {
                if (SummonRotationAdjustments.ContainsKey(projectile.type))
                    savedRotation = projectile.rotation;

                if (projectile.type == ModContent.ProjectileType<CinderBlossom>())
                    savedRotation = projectile.rotation;
            }

            if (CataclysmConfig.Instance.abyssMinesExplode && projectile.type == ModContent.ProjectileType<AbyssMine>())
                if (!NPC.AnyNPCs(ModContent.NPCType<SlimeGodCore>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGod>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()))
                    projectile.Kill();

            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            if (CataclysmConfig.Instance.drataliornusArrowsThroughBlocks && projectile.type == ModContent.ProjectileType<DrataliornusFlame>())
                projectile.tileCollide = false;

            Player player = Main.player[projectile.owner];

            if (CataclysmConfig.Instance.smootherMinionRotation)
            {
                if (SummonRotationAdjustments.ContainsKey(projectile.type))
                {
                    projectile.rotation = savedRotation;

                    if (projectile.type == ModContent.ProjectileType<PowerfulRaven>())
                    {
                        projectile.spriteDirection = 1;
                        LerpToRotation(projectile, SummonRotationAdjustments[projectile.type], 0);
                    }
                    else
                        LerpToRotation(projectile, SummonRotationAdjustments[projectile.type]);
                }

                if (projectile.type == ModContent.ProjectileType<CinderBlossom>())
                    projectile.rotation = savedRotation + (MathHelper.ToRadians(1.5f) + (player.velocity.X / player.maxRunSpeed / 5f * player.direction)) * player.direction;

                if (projectile.type == ModContent.ProjectileType<CalamariMinion>())
                {
                    projectile.rotation = savedRotation;

                    NPC target = null;

                    if (player.HasMinionAttackTargetNPC)
                        target = Main.npc[player.MinionAttackTargetNPC];
                    else
                        foreach (NPC npc in Main.npc)
                        {
                            if (npc.CanBeChasedBy(projectile))
                            {
                                target = npc;
                                break;
                            }
                        }

                    if (target != null)
                    {
                        Vector2 npcPos = target.position + target.Size * new Vector2(0.5f, 0f);
                        Vector2 wantedRotation = npcPos - projectile.Center;
                        wantedRotation.Normalize();
                        wantedRotation *= 12f;

                        projectile.rotation = projectile.rotation.AngleTowards(wantedRotation.ToRotation() - MathHelper.PiOver2, 0.2f);
                    }
                }
            }
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<FungalClumpMinion>())
            {
                if (CataclysmConfig.Instance.fungalClumpTrueDamage)
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

                if (CataclysmConfig.Instance.fungalClumpEmitsLight)
                    Lighting.AddLight(projectile.position, 22f / 200f, 54f / 255f, 125f / 255f);
            }
        }

        public static void LerpToRotation(Projectile projectile, float amount, float adjustment = MathHelper.PiOver2) => projectile.rotation = projectile.rotation.AngleTowards(projectile.velocity.ToRotation() - adjustment, amount);
    }
}