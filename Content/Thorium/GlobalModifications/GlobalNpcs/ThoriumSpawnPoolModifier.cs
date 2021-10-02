using System.Collections.Generic;
using CataclysmMod.Content.Default.GlobalModifications;
using CataclysmMod.Core.ModCompatibility;
using Terraria.ModLoader;
using ThoriumMod.NPCs.Depths;

namespace CataclysmMod.Content.Thorium.GlobalModifications.GlobalNpcs
{
    [ModDependency("ThoriumMod")]
    public class ThoriumSpawnPoolModifier : CataclysmGlobalNpc
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (pool.ContainsKey(ModContent.NPCType<Globee>())) 
                pool[ModContent.NPCType<Globee>()] *= 2f;
        }
    }
}