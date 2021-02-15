using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using CataclysmMod.Content.Configs;
using Terraria;

namespace CataclysmMod.Common.Detours
{
    public class TorrentialTearDetour : Detour
    {
        public override string DictKey => "CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem";

        public override void Load() => On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem += RemoveDeathModeCrap;

        public override void Unload() => On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem -= RemoveDeathModeCrap;

        private bool RemoveDeathModeCrap(On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.orig_UseItem orig, TorrentialTear self, object player)
        {
            if (CalamityChangesConfig.Instance.torrentialTearNerfRemoval)
            {
                if (!Main.raining)
                    CalamityUtils.StartRain(torrentialTear: true);
                else
                    Main.raining = false;

                CalamityNetcode.SyncWorld();

                return true;
            }
            else
                return orig(self, player);
        }
    }
}