using System.Collections.Generic;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class AnomuraFungusGlobal : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (!pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }
    }
}
