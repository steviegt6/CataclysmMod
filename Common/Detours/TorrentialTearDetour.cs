using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using Terraria;

namespace CataclysmMod.Common.Detours
{
    public static class TorrentialTearDetour
    {
        public static bool RemoveDeathModeCrap(On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.orig_UseItem orig, TorrentialTear self, object player)
        {
            if (!Main.raining)
                CalamityUtils.StartRain(torrentialTear: true);
            else
                Main.raining = false;

            CalamityNetcode.SyncWorld();

            return true;
        }
    }
}