using System.Collections.Generic;
using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SulphurousSea;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.Items.Accessories;
using CataclysmMod.Content.Items.Weapons;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CataclysmMod.Content.NPCs.GlobalModifications
{
    public class CalamityCompatGlobalNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Clinger:
                    if (CataclysmConfig.Instance.daggerOfDecree)
                        DropHelper.DropItemChance(npc, ModContent.ItemType<DaggerofDecree>(), Main.expertMode ? 100 : 150);
                    break;

                case NPCID.TravellingMerchant:
                    if (CataclysmConfig.Instance.pulseBowDrop)
                        DropHelper.DropItemCondition(npc, ItemID.PulseBow, Main.hardMode && Main.rand.NextBool(10));
                    break;
            }

            if (CataclysmConfig.Instance.npcsDropSharkFins)
            {
                if (npc.type == ModContent.NPCType<Frogfish>())
                    DropHelper.DropItemChance(npc, ItemID.SharkFin, 1f / 3f);

                if (npc.type == ModContent.NPCType<Catfish>())
                    DropHelper.DropItemChance(npc, ItemID.SharkFin, 2f / 3f);

                if (npc.type == ModContent.NPCType<DevilFish>() || npc.type == ModContent.NPCType<DevilFishAlt>())
                    DropHelper.DropItemChance(npc, ItemID.SharkFin, 0.75f, 1, 3);

                if (npc.type == ModContent.NPCType<FusionFeeder>())
                    DropHelper.DropItemChance(npc, ItemID.SharkFin, 0.125f);

                if (npc.type == ModContent.NPCType<Sunskater>())
                    DropHelper.DropItemChance(npc, ItemID.SharkFin, 0.10f);
            }

            if (CataclysmConfig.Instance.angryDogSpawnBuff && npc.type == ModContent.NPCType<AngryDog>())
                DropHelper.DropItemCondition(npc, ModContent.ItemType<Cryophobia>(), CataclysmConfig.Instance.angryDogSpawnBuff, 0.15f);

            if (CataclysmConfig.Instance.grandSharkRepellent && npc.type == ModContent.NPCType<GreatSandShark>() && Main.rand.NextBool(3))
                Item.NewItem(npc.Hitbox, ModContent.ItemType<GrandSharkRepellent>());
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            foreach (int npc in pool.Keys)
                if (CataclysmConfig.Instance.angryDogSpawnBuff && npc == ModContent.NPCType<AngryDog>())
                    pool[npc] = 0.024f;

            if (CataclysmConfig.Instance.anomuraFungusSpawning && !pool.ContainsKey(NPCID.AnomuraFungus) && spawnInfo.player.ZoneGlowshroom)
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
            if (CataclysmConfig.Instance.wizardGuideVoodooDoll)
                CalamityGlobalTownNPC.SetShopItem(ref shop, ref nextSlot, ItemID.GuideVoodooDoll, Main.hardMode, Item.sellPrice(gold: 20));
        }

        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            string ganicText = "";

            if (npc.Organic())
                ganicText = "Organic";
            else if (npc.Inorganic())
                ganicText = "Inorganic";

            if (!string.IsNullOrEmpty(ganicText) && CataclysmConfig.Instance.displayOrganicTextNPCs)
                ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, ganicText, position - Main.screenPosition - new Vector2(Main.fontMouseText.MeasureString(ganicText).X / 2f, -(Main.fontMouseText.MeasureString(ganicText).Y / 2f)), Lighting.GetColor((int)(npc.position.X / 16), (int)(npc.position.Y / 16)), 0f, Vector2.Zero, Vector2.One);

            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }
    }
}