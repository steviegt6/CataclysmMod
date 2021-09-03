using System.Collections.Generic;
using System.Linq;
using CalamityMod.Projectiles.Summon;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalProjectiles
{
    public class CalamityMinionRotationHandler : CalamityGlobalProjectileBase
    {
        public Dictionary<int, RotationData> SummonRotationAdjustments => new Dictionary<int, RotationData>
        {
            {ModContent.ProjectileType<IceClasperMinion>(), new RotationData(0.15f)},
            {ModContent.ProjectileType<PowerfulRaven>(), new RotationData(0.25f, 0f)},
            {ModContent.ProjectileType<HerringMinion>(), new RotationData(0.25f)}
        };

        public float SavedRotation;

        public override bool InstancePerEntity => true;

        public override bool CloneNewInstances => true;

        public static void LerpToRotation(Projectile projectile, RotationData rotData)
        {
            if (!float.IsNaN(rotData.RotationAdjustment))
                projectile.rotation =
                    projectile.rotation.AngleTowards(projectile.velocity.ToRotation() - rotData.RotationAdjustment,
                        rotData.RotationAmount);
        }

        public override bool PreAI(Projectile projectile)
        {
            if (SummonRotationAdjustments.ContainsKey(projectile.type))
                SavedRotation = projectile.rotation;

            return base.PreAI(projectile);
        }

        public override void PostAI(Projectile projectile)
        {
            Player player = Main.player[projectile.owner];

            if (!SummonRotationAdjustments.ContainsKey(projectile.type))
                return;

            RotationData data = SummonRotationAdjustments[projectile.type];
            projectile.rotation = SavedRotation;

            LerpToRotation(projectile, data);

            if (data.SpriteDirectionOverride != 0)
                projectile.spriteDirection = data.SpriteDirectionOverride;

            if (projectile.type == ModContent.ProjectileType<CinderBlossom>())
            {
                float rotationOffsetBySpeed = MathHelper.ToRadians(1.5f) +
                                              player.velocity.X / player.maxRunSpeed / 5f * player.direction;
                rotationOffsetBySpeed *= player.direction;
                projectile.rotation = SavedRotation + rotationOffsetBySpeed;
            }

            if (projectile.type == ModContent.ProjectileType<CalamariMinion>())
            {
                projectile.rotation = SavedRotation;
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
            public readonly float RotationAmount;
            public readonly float RotationAdjustment;
            public readonly int SpriteDirectionOverride;

            public RotationData(float rotationAmount, float rotationAdjustment = MathHelper.PiOver2,
                int spriteDirectionOverride = 0)
            {
                RotationAmount = rotationAmount;
                RotationAdjustment = rotationAdjustment;
                SpriteDirectionOverride = spriteDirectionOverride;
            }
        }
    }
}