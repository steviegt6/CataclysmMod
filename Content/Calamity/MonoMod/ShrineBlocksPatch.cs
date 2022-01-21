using System;
using System.Reflection;
using CalamityMod.World;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.MonoMod;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    public class ShrineBlocksPatch : MonoModPatcher<string>
    {
        public override MethodInfo Method => typeof(UndergroundShrines).GetCachedMethod(nameof(UndergroundShrines.PlaceShrines));

        public override string ModderMethod => nameof(ChangeCavernShrineBlocks);

        public static void ChangeCavernShrineBlocks(ILContext il)
        {
            void DoPatch(ILCursor cursor)
            {
                /* Match the first number in the call for WorldGenerationMethods.SpecialHut for cavern shrines (75)
                 * // WorldGenerationMethods.SpecialHut(75, 56, 20, 2, num8, num10);
                 * IL_0225: ldc.i4.s 75 // Cavern shrine tile
                 * IL_0227: ldc.i4.s 56 // Cavern shrine tile
                 * IL_0229: ldc.i4.s 20 // Cavern shrine tile
                 * IL_022b: ldc.i4.2
                 * IL_022c: ldloc.s 13
                 * IL_022e: ldloc.s 15
                 */
                if (!cursor.TryGotoNext(i => i.MatchLdcI4(75)))
                {
                    throw new Exception("Patch failure: ldc.i4.s -> 75");
                }

                cursor.Index++;

                // Pop the normal values and replace them with the IDs for stone bricks and stone
                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldc_I4, TileID.GrayBrick);

                cursor.Index++;

                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldc_I4, TileID.Stone);

                cursor.Index++;

                cursor.Emit(OpCodes.Pop);
                cursor.Emit(OpCodes.Ldc_I4, (int) WallID.GrayBrick);
            }

            ILCursor c = new ILCursor(il);

            DoPatch(c);
            DoPatch(c);
        }
    }
}