using System.Collections.Generic;
using System.Linq;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Projectiles
{
    public class SummonRotationAdjustmentsGlobalProj : GlobalProjectile
    {
        public static Dictionary<int, RotationData> SummonRotationAdjustments;

        private float _savedRotation;

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public static void LerpToRotation(Projectile projectile, RotationData rotData)
        {
            if (!float.IsNaN(rotData.rotationAdjustment))
                projectile.rotation =
                    projectile.rotation.AngleTowards(projectile.velocity.ToRotation() - rotData.rotationAdjustment,
                        rotData.rotationAmount);
        }

        internal static void Initialize()
        {
            SummonRotationAdjustments = new Dictionary<int, RotationData>
            {
                { ModContent.ProjectileType<IceClasperMinion>(), new RotationData(0.15f) },
                { ModContent.ProjectileType<PowerfulRaven>(), new RotationData(0.25f, 0f) },
                { ModContent.ProjectileType<HerringMinion>(), new RotationData(0.25f) }
            };
        }

        public override bool PreAI(Projectile projectile)
        {
            if (SummonRotationAdjustments.ContainsKey(projectile.type))
                _savedRotation = projectile.rotation;

            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (!SummonRotationAdjustments.ContainsKey(projectile.type))
                return;

            RotationData data = SummonRotationAdjustments[projectile.type];
            projectile.rotation = _savedRotation;

            LerpToRotation(projectile, data);

            if (data.spriteDirectionOverride != 0)
                projectile.spriteDirection = data.spriteDirectionOverride;

            if (projectile.type == ModContent.ProjectileType<CinderBlossom>())
            {
                float rotationOffsetBySpeed = MathHelper.ToRadians(1.5f) +
                                              player.velocity.X / player.maxRunSpeed / 5f * player.direction;
                rotationOffsetBySpeed *= player.direction;
                projectile.rotation = _savedRotation + rotationOffsetBySpeed;
            }

            if (projectile.type == ModContent.ProjectileType<CalamariMinion>())
            {
                projectile.rotation = _savedRotation;
                NPC target = player.HasMinionAttackTargetNPC
                    ? Main.npc[player.MinionAttackTargetNPC]
                    : Main.npc.FirstOrDefault(n => n.CanBeChasedBy(projectile));

                if (target == null)
                    return;

                Vector2 npcPos = target.position + target.Size * new Vector2(0.5f, 0f);
                Vector2 wantedRotation = npcPos - projectile.Center;
                wantedRotation.Normalize();
                wantedRotation *= 12f;

                projectile.rotation =
                    projectile.rotation.AngleTowards(wantedRotation.ToRotation() - MathHelper.PiOver2, 0.2f);
            }
        }

        public readonly struct RotationData
        {
            public readonly float rotationAmount;
            public readonly float rotationAdjustment;
            public readonly int spriteDirectionOverride;

            public RotationData(float rotationAmount, float rotationAdjustment = MathHelper.PiOver2,
                int spriteDirectionOverride = 0)
            {
                this.rotationAmount = rotationAmount;
                this.rotationAdjustment = rotationAdjustment;
                this.spriteDirectionOverride = spriteDirectionOverride;
            }
        }
    }
}