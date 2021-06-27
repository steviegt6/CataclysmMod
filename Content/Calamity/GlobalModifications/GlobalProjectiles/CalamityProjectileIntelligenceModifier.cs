using System.Collections.Generic;
using System.Linq;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Projectiles.Boss;
using CalamityMod.Projectiles.Ranged;
using CalamityMod.Projectiles.Summon;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalProjectiles
{
    [ModDependency("CalamityMod")]
    public class CalamityProjectileIntelligenceModifier : CataclysmGlobalProjectile
    {
        public List<int> SlimeGodNpcs { get; }
        public List<int> AbyssMines { get; }

        public CalamityProjectileIntelligenceModifier()
        {
            SlimeGodNpcs = new List<int>
            {
                ModContent.NPCType<SlimeGodCore>(),
                ModContent.NPCType<SlimeGod>(),
                ModContent.NPCType<SlimeGodSplit>(),
                ModContent.NPCType<SlimeGodRun>()
            };

            AbyssMines = new List<int>
            {
                ModContent.ProjectileType<AbyssMine>(),
                ModContent.ProjectileType<AbyssMine2>()
            };
        }

        public override bool PreAI(Projectile projectile)
        {
            if (!AbyssMines.Contains(projectile.type))
                return base.PreAI(projectile);

            bool slimeGodAlive = false;

            foreach (int _ in SlimeGodNpcs.Where(NPC.AnyNPCs))
                slimeGodAlive = true;

            if (!slimeGodAlive)
                projectile.Kill();

            return base.PreAI(projectile);
        }

        public override void AI(Projectile projectile)
        {
            if (projectile.type != ModContent.ProjectileType<FungalClumpMinion>())
                return;

            Vector3 light = new Vector3(22f / 200f, 54f / 255f, 125f / 255f); // weird calculations man
            Lighting.AddLight(projectile.position, light);
        }

        public override void PostAI(Projectile projectile)
        {
            if (projectile.type == ModContent.ProjectileType<DrataliornusFlame>())
                projectile.tileCollide = false;
        }
    }
}