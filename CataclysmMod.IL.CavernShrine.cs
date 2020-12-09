using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        private void ChanceCavernShrineChest(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(44)))
            {
                Logger.Warn("[IL] Unable to match ldc.i4.s \"44\"!");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, 1);

            Logger.Info("[IL] Finished patching!");
        }

        private void ChangeCavernShrineBlocks(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(75)))
            {
                Logger.Warn("[IL] Unable to match ldc.i4.s \"75\"! (1)");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, TileID.GrayBrick);

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, TileID.Stone);

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, (int)WallID.GrayBrick);

            if (!c.TryGotoNext(i => i.MatchLdcI4(75)))
            {
                Logger.Warn("[IL] Unable to match ldc.i4.s \"75\"! (2)");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, TileID.GrayBrick);

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, TileID.Stone);

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, (int)WallID.GrayBrick);

            Logger.Info("[IL] Finished patching!");
        }
    }
}
