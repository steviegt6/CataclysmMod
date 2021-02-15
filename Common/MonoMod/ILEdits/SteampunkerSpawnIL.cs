using System;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Configs;
using IL.CalamityMod.World;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class SteampunkerSpawnIL : ILEdit
    {
        public override string DictKey => "CalamityMod.World.CalamityWorld.PostUpdate";

        public override bool Autoload() => CataclysmConfig.Instance.steampunkerSpawnFix;

        public override void Load() => CalamityWorld.PostUpdate += ModifySteampunkerSpawn;

        public override void Unload() => CalamityWorld.PostUpdate -= ModifySteampunkerSpawn;

        private static void ModifySteampunkerSpawn(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(x => x.MatchLdcI4(NPCID.Steampunker)))
            {
                ILLogger.LogILError("ldc.i4", "178");
                return;
            }

            if (!c.TryGotoNext(x => x.MatchLdloc(0)))
            {
                ILLogger.LogILError("ldloc", "0");
                return;
            }

            // Remove the entire method call
            c.RemoveRange(3);

            c.Emit(OpCodes.Ldloc_0); // num

            // Insert our own method call
            c.EmitDelegate<Action<int>>(whoAmI =>
            {
                Player player = Main.player[whoAmI];
                NPC.NewNPC((int)player.position.X, (int)player.position.Y, NPCID.Steampunker);
            });

            ILLogger.LogILCompletion("CalamityMod.World.CalamityWorld.PostUpdate");
        }
    }
}