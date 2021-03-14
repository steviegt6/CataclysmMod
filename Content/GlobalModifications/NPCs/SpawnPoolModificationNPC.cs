using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using CataclysmMod.Content.Configs;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.NPCs
{
    public class SpawnPoolModificationNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (int npc in pool.Keys)
                if (CataclysmConfig.Instance.angryDogSpawnBuff && npc == ModContent.NPCType<AngryDog>())
                    pool[npc] = 0.024f;

            if (CataclysmConfig.Instance.anomuraFungusSpawning && !pool.ContainsKey(NPCID.AnomuraFungus) &&
                spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }
    }
}