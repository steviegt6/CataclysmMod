using System.Reflection;
using CalamityMod.World;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class WorldGenerationMethodsSpecialChest : MonoModExecutor<MethodInfo>
    {
        public override MethodInfo Method =>
            typeof(WorldGenerationMethods).GetMethodForced(nameof(WorldGenerationMethods.SpecialChest));

        public override MethodInfo ModderMethod => GetType().GetMethodForced(nameof(ChangeCavernShrineChest));

        public override void Apply()
        {
            if (CataclysmConfig.Instance.CavernShrineChanges)
                base.Apply();
        }

        public static void ChangeCavernShrineChest(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            /* Match the specific chest style of obsidian chests, the style used for the cavern shrines normally. (Style 44)
             * // contain = ModContent.ItemType<OnyxExcavatorKey>();
             * IL_005b: call int32 [Terraria]Terraria.ModLoader.ModContent::ItemType<class CalamityMod.Items.Mounts.OnyxExcavatorKey>()
             * IL_0060: stloc.0
             * // style = 44;
             * IL_0061: ldc.i4.s 44
             * IL_0063: stloc.1
             * // break;
             * IL_0064: br.s IL_00a5
             */
            if (!c.TryGotoNext(i => i.MatchLdcI4(44)))
            {
                LoggerUtils.LogPatchError("ldc.i4.s", "44");
                return;
            }

            c.Index++;

            // Pop 44 and replace it with 1, the style for gold chests
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, 1);

            LoggerUtils.LogPatchCompletion("IL.CalamityMod.World.WorldGenerationMethods.SpecialChest");
        }
    }

    public class SmallBiomesPlaceShrines : MonoModExecutor<MethodInfo>
    {
        public override MethodInfo Method => typeof(SmallBiomes).GetMethodForced(nameof(SmallBiomes.PlaceShrines));

        public override MethodInfo ModderMethod => GetType().GetMethodForced(nameof(ChangeCavernShrineBlocks));

        public override void Apply()
        {
            if (CataclysmConfig.Instance.CavernShrineChanges)
                base.Apply();
        }

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
                    LoggerUtils.LogPatchError("ldc.i4.s", "75", 1 + 1);
                    return;
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

            LoggerUtils.LogPatchCompletion("IL.CalamityMod.World.SmallBiomes.PlaceShrines");
        }
    }
}