using CalamityMod.NPCs.GreatSandShark;
using CataclysmMod.Common.Configs;
using CataclysmMod.Content.Items.Accessories;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class GreatSandSharkGlobal : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            if (CalamityChangesConfig.Instance.grandSharkRepellent && npc.type == ModContent.NPCType<GreatSandShark>() && Main.rand.NextBool(3))
                Item.NewItem(npc.Hitbox, ModContent.ItemType<GrandSharkRepellent>());
        }
    }
}