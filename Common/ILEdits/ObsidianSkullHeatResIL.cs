using CalamityMod.CalPlayer;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using System.Reflection;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.Players;
using Terraria;

namespace CataclysmMod.Common.ILEdits
{
    public class ObsidianSkullHeatResIL : ILEdit
    {
        public override string DictKey => "CalamityMod.CalPlayer.CalamityPlayerMiscEffects.MiscEffects";

        private event ILContext.Manipulator MiscEffectsHook
        {
            add => HookEndpointManager.Modify(typeof(CalamityPlayerMiscEffects).GetMethod("MiscEffects", BindingFlags.Static | BindingFlags.NonPublic), value);

            remove => HookEndpointManager.Unmodify(typeof(CalamityPlayerMiscEffects).GetMethod("MiscEffects", BindingFlags.Static | BindingFlags.NonPublic), value);
        }

        public override void Load() => MiscEffectsHook += AddTheFunny;

        public override void Unload() => MiscEffectsHook -= AddTheFunny;

        private void AddTheFunny(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdfld(typeof(CalamityPlayer).GetField("externalHeatImmunity", BindingFlags.Instance | BindingFlags.Public))))
            {
                CataclysmMod.Instance.Logger.Warn("externalHeatImmunity");
                return;
            }

            c.Index += 2;

            c.Emit(OpCodes.Stloc, 17);
            c.Emit(OpCodes.Ldarg_0);
            c.EmitDelegate<Action<bool, Player>>((funnyLavaHeatRes, player) =>
            {
                if (player.GetModPlayer<CalamityCompatPlayer>().obsidianSkullIsFunny && CalamityChangesConfig.Instance.obsidianSkullHeatImmunity)
                    funnyLavaHeatRes = true;
            });
        }
    }
}