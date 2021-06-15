using System.Reflection;
using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using Terraria;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class TorrentialTearUseItem : MonoModExecutor<string>
    {
        public override MethodInfo Method => typeof(TorrentialTear).GetMethodForced(nameof(TorrentialTear.UseItem));

        public override string ModderMethod => nameof(RemoveDeathModeCrap);

        public static bool RemoveDeathModeCrap(
            On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.orig_UseItem orig, TorrentialTear self,
            object player)
        {
            if (!CataclysmConfig.Instance.TorrentialTearNerfRemoval)
                return orig(self, player);

            if (!Main.raining)
                CalamityUtils.StartRain(true);
            else
                Main.raining = false;

            CalamityNetcode.SyncWorld();

            return true;
        }
    }
}