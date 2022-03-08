#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CalamityMod;
using CalamityMod.Items.Tools.ClimateChange;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Weaving;
using Terraria;

namespace CataclysmMod.Content.CalamityMod.Patches
{
    [AddonContent(typeof(CalamityModAddon))]
    public class TorrentialTearOverriderPatch : ILoadable
    {
        public void Load()
        {
            HookCreator.Detour(
                typeof(TorrentialTear).GetCachedMethod(nameof(TorrentialTear.UseItem)),
                GetType().GetCachedMethod(nameof(RemoveDeathModeCrap))
            );
        }
        
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