﻿using CalamityMod;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.NPCs.Abyss;
using CalamityMod.NPCs.Astral;
using CalamityMod.NPCs.GreatSandShark;
using CalamityMod.NPCs.NormalNPCs;
using CalamityMod.NPCs.SulphurousSea;
using CataclysmMod.Content.Items.Accessories;
using CataclysmMod.Content.Items.Weapons;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.NPCs
{
    public class DropHandlerNPC : GlobalNPC
    {
        public override void NPCLoot(NPC npc)
        {
            switch (npc.type)
            {
                case NPCID.Clinger:
                    DropHelper.DropItemChance(npc, ModContent.ItemType<DaggerofDecree>(),
                        Main.expertMode ? 100 : 150);
                    break;

                case NPCID.TravellingMerchant:
                    DropHelper.DropItemCondition(npc, ItemID.PulseBow,
                        Main.hardMode && NPC.downedPlantBoss && Main.rand.NextBool(10));
                    break;
            }

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

            if (npc.type == ModContent.NPCType<AngryDog>())
                DropHelper.DropItemChance(npc, ModContent.ItemType<Cryophobia>(), 0.15f);

            if (npc.type == ModContent.NPCType<GreatSandShark>() && Main.rand.NextBool(3))
                Item.NewItem(npc.Hitbox, ModContent.ItemType<GrandSharkRepellent>());
        }
    }
}