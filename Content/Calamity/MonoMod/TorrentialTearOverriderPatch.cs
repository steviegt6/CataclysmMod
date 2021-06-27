using System.Reflection;
using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.MonoMod;
using Terraria;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    public class TorrentialTearOverriderPatch : MonoModPatcher<MethodInfo>
    {
        public override MethodInfo Method => typeof(TorrentialTear).GetCachedMethod(nameof(TorrentialTear.UseItem));

        public override MethodInfo ModderMethod => GetType().GetCachedMethod(nameof(RemoveDeathModeCrap));

        public static bool RemoveDeathModeCrap(TorrentialTear self, Player player)
        {
            if (!Main.raining)
                CalamityUtils.StartRain(true);
            else
                Main.raining = false;

            CalamityNetcode.SyncWorld();

            return true;
        }
    }
}