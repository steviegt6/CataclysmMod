using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.NPCs.NormalNPCs;
using CataclysmMod.Common.Configs;
using CataclysmMod.Content.Items.Accessories;
using CataclysmMod.Content.Items.Weapons;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class CalamityCompatGlobalNPC : GlobalNPC
    {
        public override bool Autoload(ref string name) => CataclysmMod.Instance.Calamity != null;

        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Clinger:
                    if (CalamityChangesConfig.Instance.daggerOfDecree)
                        DropHelper.DropItemChance(npc, ModContent.ItemType<DaggerofDecree>(), Main.expertMode ? 100 : 150);
                    break;
            }

            if (CalamityChangesConfig.Instance.angryDogSpawnBuff && npc.type == ModContent.NPCType<AngryDog>())
                DropHelper.DropItemCondition(npc, ModContent.ItemType<Cryophobia>(), CalamityChangesConfig.Instance.angryDogSpawnBuff, 0.15f);

            if (CalamityChangesConfig.Instance.grandSharkRepellent && npc.type == ModContent.NPCType<GreatSandShark>() && Main.rand.NextBool(3))
                Item.NewItem(npc.Hitbox, ModContent.ItemType<GrandSharkRepellent>());
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (int npc in pool.Values)
                if (CalamityChangesConfig.Instance.angryDogSpawnBuff && npc == ModContent.NPCType<AngryDog>())
                    pool[npc] = 0.024f;

            if (CalamityChangesConfig.Instance.anomuraFungusSpawning && !pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
                pool.Add(NPCID.AnomuraFungus, 0.1f);
        }

        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            switch (type)
            {
                case NPCID.Wizard:
                    SetupWizardShop(shop, ref nextSlot);
                    break;
            }
        }

        public void SetupWizardShop(Chest shop, ref int nextSlot)
        {
            if (CalamityChangesConfig.Instance.wizardGuideVoodooDoll)
                CalamityGlobalTownNPC.SetShopItem(ref shop, ref nextSlot, ItemID.GuideVoodooDoll, Main.hardMode, Item.sellPrice(gold: 20));
        }
    }
}