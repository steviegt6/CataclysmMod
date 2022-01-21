using System;
using System.Reflection;
using CalamityMod.World;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.MonoMod;
using Mono.Cecil.Cil;
using MonoMod.Cil;

namespace CataclysmMod.Content.Calamity.MonoMod
{
    public class ShrineChestPatch : MonoModPatcher<string>
    {
        public override MethodInfo Method =>
            typeof(UndergroundShrines).GetCachedMethod(nameof(UndergroundShrines.SpecialChest));

        public override string ModderMethod => nameof(ChangeCavernShrineChest);

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
                throw new Exception("Patch failure: ldc.i4.s -> 44");
            }

            c.Index++;

            // Pop 44 and replace it with 1, the style for gold chests
            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, 1);
        }
    }
}