using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using CataclysmMod.Common.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalNpcs
{
    public class CalamitySpawnPoolModifier : CalamityGlobalNpcBase
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            pool = pool.ModifyIfContains(ModContent.NPCType<AngryDog>(), 0.024f); // 2* normal chance

            if (!pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }
    }
}