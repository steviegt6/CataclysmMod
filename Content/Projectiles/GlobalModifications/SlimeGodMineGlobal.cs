using CalamityMod.NPCs.SlimeGod;
using CalamityMod.Projectiles.Boss;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Projectiles.GlobalModifications
{
    public class SlimeGodMineGlobal : GlobalProjectile
    {
        public override void PostAI(Projectile projectile)
        {
            if (CalamityChangesConfig.Instance.abyssMinesExplode && projectile.type == ModContent.ProjectileType<AbyssMine>())
                if (!NPC.AnyNPCs(ModContent.NPCType<SlimeGodCore>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGod>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodSplit>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRun>()) && !NPC.AnyNPCs(ModContent.NPCType<SlimeGodRunSplit>()))
                    projectile.Kill();
        }
    }
}