using System.Collections.Generic;
using System.Linq;
using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Projectiles.Boss;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Projectiles
{
    public class AbyssalMinesExplosionGlobalProj : GlobalProjectile
    {
        public static List<int> SlimeGodNPCs;
        public static List<int> AbyssMines;

        internal static void Initialize()
        {
            SlimeGodNPCs = new List<int>
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

            foreach (int _ in SlimeGodNPCs.Where(NPC.AnyNPCs))
                slimeGodAlive = true;

            if (!slimeGodAlive)
                projectile.Kill();

            return base.PreAI(projectile);
        }
    }
}