using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalNpcs
{
    [ModDependency("CalamityMod")]
    public class CalamitySpawnPoolModifier : CataclysmGlobalNpc
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            pool = pool.ModifyIfContains(ModContent.NPCType<AngryDog>(), 0.024f); // 2* normal chance

            if (!pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }
    }
}