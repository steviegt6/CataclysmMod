using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        private void RemoveSummonDamageBonus(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcR4(10)))
            {
                Logger.Warn("[IL] Unable to match ldc.r4 \"10\"!");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, 10);

            c.Index++;

            c.RemoveRange(4);

            Logger.Info("[IL] Finished patching!");
        }
    }
}
