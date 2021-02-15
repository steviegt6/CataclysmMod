using System;
using System.Reflection;
using CalamityMod.CalPlayer;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.GlobalModifications.Players;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class ObsidianSkullHeatResIL : ILEdit
    {
        public override string DictKey => "CalamityMod.CalPlayer.CalamityPlayerMiscEffects.MiscEffects";

        private event ILContext.Manipulator MiscEffectsHook
        {
            add =>
                HookEndpointManager.Modify(
                    typeof(CalamityPlayerMiscEffects).GetMethod("MiscEffects",
                        BindingFlags.Static | BindingFlags.NonPublic), value);

            remove =>
                HookEndpointManager.Unmodify(
                    typeof(CalamityPlayerMiscEffects).GetMethod("MiscEffects",
                        BindingFlags.Static | BindingFlags.NonPublic), value);
        }

        public override void Load() => MiscEffectsHook += AddObsidianSkullCraftingTreeHeatImmunity;

        public override void Unload() => MiscEffectsHook -= AddObsidianSkullCraftingTreeHeatImmunity;

        private static void AddObsidianSkullCraftingTreeHeatImmunity(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdfld(typeof(CalamityPlayer).GetField("externalHeatImmunity",
                                   BindingFlags.Instance | BindingFlags.Public))))
            {
                ILLogger.LogILError("ldfld", "CalamityMod.CalPlayer.CalamityPlayer::externalHeatImmunity");
                return;
            }

            c.Index += 2;

            c.Emit(OpCodes.Stloc, 17);
            c.Emit(OpCodes.Ldarg_0);

            // ReSharper disable once RedundantAssignment
            c.EmitDelegate<Action<bool, Player>>((funnyLavaHeatRes, player) =>
                                                 {
                                                     if (player.GetModPlayer<CalamityCompatPlayer>()
                                                             .playerHasObsidianSkullOrTree &&
                                                         CataclysmConfig.Instance.obsidianSkullHeatImmunity)
                                                         // ReSharper disable once RedundantAssignment
                                                         funnyLavaHeatRes = true;
                                                 });

            ILLogger.LogILCompletion("CalamityMod.CalPlayer.CalamityMiscEffects.MiscEffects");
        }
    }
}