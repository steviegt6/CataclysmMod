using CalamityMod;
using CataclysmMod.Content.Configs;
using On.CalamityMod.Items.Tools.ClimateChange;
using Terraria;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class TorrentialTearDetour : ILEdit
    {
        public override string DictKey => "CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem";

        public override void Load() => TorrentialTear.UseItem += RemoveDeathModeCrap;

        public override void Unload() => TorrentialTear.UseItem -= RemoveDeathModeCrap;

        private static bool RemoveDeathModeCrap(TorrentialTear.orig_UseItem orig, CalamityMod.Items.Tools.ClimateChange.TorrentialTear self, object player)
        {
            if (!CataclysmConfig.Instance.torrentialTearNerfRemoval)
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