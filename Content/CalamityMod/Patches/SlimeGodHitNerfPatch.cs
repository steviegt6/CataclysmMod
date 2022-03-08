#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CalamityMod.NPCs.SlimeGod;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Weaving;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.CalamityMod.Patches
{
    [AddonContent(typeof(CalamityModAddon))]
    public class SlimeGodHitNerfPatch : ILoadable
    {
        public delegate void T();

        public static T A;
        public void Load()
        {
            HookCreator.Detour(
                typeof(SlimeGodCore).GetCachedMethod(nameof(SlimeGodCore.OnHitPlayer)),
                GetType().GetCachedMethod(nameof(NewOnHitBuff))
            );
        }
        
        public static void NewOnHitBuff(SlimeGodCore self, Player player, int damage, bool crit)
        {
            player.AddBuff(BuffID.Slimed, 60 * 2);
        }
    }
}