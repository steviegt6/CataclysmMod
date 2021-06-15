using System;
using System.Reflection;
using CalamityMod.CalPlayer;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.GlobalModifications.Players;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using Terraria;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class CalamityPlayerMiscEffectsMiscEffects : MonoModExecutor<MethodInfo>
    {
        public override MethodInfo Method => typeof(CalamityPlayerMiscEffects).GetMethodForced("MiscEffects");

        public override MethodInfo ModderMethod =>
            GetType().GetMethodForced(nameof(AddObsidianSkullCraftingTreeHeatImmunity));

        public static void AddObsidianSkullCraftingTreeHeatImmunity(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdfld(typeof(CalamityPlayer).GetField("externalHeatImmunity",
                BindingFlags.Instance | BindingFlags.Public))))
            {
                LoggerUtils.LogPatchError("ldfld", "CalamityMod.CalPlayer.CalamityPlayer::externalHeatImmunity");
                return;
            }

            c.Index += 2;

            c.Emit(OpCodes.Stloc, 17);
            c.Emit(OpCodes.Ldarg_0);

            // ReSharper disable once RedundantAssignment
            c.EmitDelegate<Action<bool, Player>>((funnyLavaHeatRes, player) =>
            {
                if (player.GetModPlayer<CalamityCompatPlayer>().playerHasObsidianSkullOrTree)
                    // ReSharper disable once RedundantAssignment
                    funnyLavaHeatRes = true;
            });

            LoggerUtils.LogPatchCompletion("CalamityMod.CalPlayer.CalamityMiscEffects.MiscEffects");
        }
    }
}