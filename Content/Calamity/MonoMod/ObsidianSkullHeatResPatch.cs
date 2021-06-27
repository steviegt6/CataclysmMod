using System;
using System.Reflection;
using CalamityMod.CalPlayer;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Calamity.GlobalModifications.ModPlayers;
using CataclysmMod.Content.Default.MonoMod;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    [ModDependency("CalamityMod")]
    public class ObsidianSkullHeatResPatch : MonoModPatcher<string>
    {
        public override MethodInfo Method => typeof(CalamityPlayerMiscEffects).GetCachedMethod("MiscEffects");

        public override string ModderMethod => nameof(AddObsidianSkullCraftingTreeHeatImmunity);

        public static void AddObsidianSkullCraftingTreeHeatImmunity(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdfld(typeof(CalamityPlayer).GetField("externalHeatImmunity",
                BindingFlags.Instance | BindingFlags.Public))))
            {
                throw new Exception(
                    "Patch failure: ldfld -> CalamityMod.CalPlayer.CalamityPlayer::externalHeatImmunity");
            }

            c.Index += 2;

            c.Emit(OpCodes.Stloc, 17);
            c.Emit(OpCodes.Ldarg_0);

            // ReSharper disable once RedundantAssignment
            c.EmitDelegate<Action<bool, Player>>((funnyLavaHeatRes, player) =>
            {
                if (player.GetModPlayer<CalamityCataclysmPlayer>().ObsidianSkullHeatRes)
                    // ReSharper disable once RedundantAssignment
                    funnyLavaHeatRes = true;
            });
        }
    }
}