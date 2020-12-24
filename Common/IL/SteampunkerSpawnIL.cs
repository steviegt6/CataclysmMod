using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Common.IL
{
    public static class SteampunkerSpawnIL
    {
        internal static void ModifySteampunkerSpawn(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(x => x.MatchLdcI4(NPCID.Steampunker)))
            {
                ModContent.GetInstance<CataclysmMod>().Logger.Warn($"[IL] Unable to match ldc.i4 \"178\"!");
                return;
            }

            if (!c.TryGotoNext(x => x.MatchLdloc(0)))
            {
                ModContent.GetInstance<CataclysmMod>().Logger.Warn($"[IL] Unable to match ldloc.0!");
                return;
            }

            //c.Index++;

            // Remove the entire method call
            c.RemoveRange(3);

            c.Emit(OpCodes.Ldloc_0); // num

            // Insert our own method call
            c.EmitDelegate<Action<int>>((whoAmI) =>
            {
                Player player = Main.player[whoAmI];

                NPC.NewNPC((int)player.position.X, (int)player.position.Y, NPCID.Steampunker);
            });
        }
    }
}