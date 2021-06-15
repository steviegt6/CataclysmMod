using System.Reflection;
using CalamityMod.NPCs.SlimeGod;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class SlimeGodCoreOnHitPlayer : MonoModExecutor<string>
    {
        public override MethodInfo Method => typeof(SlimeGodCore).GetMethodForced(nameof(SlimeGodCore.OnHitPlayer));

        public override string ModderMethod => nameof(NewOnHitBuff);

        public static void NewOnHitBuff(On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.orig_OnHitPlayer orig,
            SlimeGodCore self, object player, int damage, bool crit)
        {
            if (player is Player vanillaPlayer && CataclysmConfig.Instance.SlimeGodSlimedDebuff)
                vanillaPlayer.AddBuff(BuffID.Slimed, 60 * 2);
            else
                orig(self, player, damage, crit);
        }
    }
}