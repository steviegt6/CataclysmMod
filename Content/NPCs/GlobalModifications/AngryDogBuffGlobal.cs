using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.NormalNPCs;
using CataclysmMod.Common.Configs;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class AngryDogBuffGlobal : GlobalNPC
    {
        // Add 0.15 so the drop change is boosted to 2.5%.
        public override void NPCLoot(NPC npc)
        {
            if (CalamityChangesConfig.Instance.angryDogSpawnBuff && npc.type == ModContent.NPCType<AngryDog>())
                DropHelper.DropItemCondition(npc, ModContent.ItemType<Cryophobia>(), CalamityChangesConfig.Instance.angryDogSpawnBuff, 0.15f);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (int npc in pool.Values)
                if (CalamityChangesConfig.Instance.angryDogSpawnBuff && npc == ModContent.NPCType<AngryDog>())
                    pool[npc] = 0.024f;
        }
    }
}