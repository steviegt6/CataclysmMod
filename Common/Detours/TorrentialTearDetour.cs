using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using CataclysmMod.Common.Configs;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using Terraria;

namespace CataclysmMod.Common.Detours
{
    public class TorrentialTearDetour : Detour
    {
        public override string DictKey => "CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem";

        public override bool Autoload() => CalamityChangesConfig.Instance.torrentialTearNerfRemoval && CataclysmMod.Instance.Calamity != null;

        public override void Load() => OnTorrentialTearUseItem += RemoveDeathModeCrap;

        public override void Unload() => OnTorrentialTearUseItem -= RemoveDeathModeCrap;

        private delegate bool OrigDelegateTorrentialTear(object self, object player);

        private delegate bool HookDelegateTorrentialTear(OrigDelegateTorrentialTear orig, object self, object player);

        private event HookDelegateTorrentialTear OnTorrentialTearUseItem
        {
            add => HookEndpointManager.Add<HookDelegateTorrentialTear>(CataclysmMod.Instance.Calamity.Code.GetType("CalamityMod.Items.Tools.ClimateChange.TorrentialTear").GetMethod("UseItem", BindingFlags.Instance | BindingFlags.Public), value);

            remove => HookEndpointManager.Remove<HookDelegateTorrentialTear>(CataclysmMod.Instance.Calamity.Code.GetType("CalamityMod.Items.Tools.ClimateChange.TorrentialTear").GetMethod("UseItem", BindingFlags.Instance | BindingFlags.Public), value);
        }

        private bool RemoveDeathModeCrap(OrigDelegateTorrentialTear orig, object self, object player)
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