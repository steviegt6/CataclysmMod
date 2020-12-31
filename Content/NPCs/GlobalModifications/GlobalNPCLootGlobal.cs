using CalamityMod;
using CataclysmMod.Common.Configs;
using CataclysmMod.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class GlobalNPCLootGlobal : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Clinger:
                    if (CalamityChangesConfig.Instance.daggerOfDecree)
                        DropHelper.DropItemChance(npc, ModContent.ItemType<DaggerofDecree>(), Main.expertMode ? 100 : 150);
                    break;
            }
        }
    }
}