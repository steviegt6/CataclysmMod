using System.Reflection;
using CalamityMod.NPCs.SlimeGod;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.MonoMod;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    public class SlimeGodHitNerfPatch : MonoModPatcher<MethodInfo>
    {
        public override MethodInfo Method => typeof(SlimeGodCore).GetCachedMethod(nameof(SlimeGodCore.OnHitPlayer));

        public override MethodInfo ModderMethod => GetType().GetCachedMethod(nameof(NewOnHitBuff));

        public static void NewOnHitBuff(SlimeGodCore self, Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Slimed, 60 * 2);
        }
    }
}