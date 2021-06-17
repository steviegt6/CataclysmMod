using System.Collections.Generic;
using CalamityMod.NPCs.NormalNPCs;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.NPCs
{
    public class SpawnPoolModificationNPC : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            ModifyIfValid(pool, ModContent.NPCType<AngryDog>(), 0.024f); // 2* normal chance

            if (!pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }

        public static void ModifyIfValid<TKey, TValue>(IDictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if (dict.ContainsKey(key))
                dict[key] = value;
        }
    }
}