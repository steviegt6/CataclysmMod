using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Content.Configs;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.GlobalModifications.Items
{
    public class ArmorSetDatabase : GlobalItem
    {
        public readonly struct ArmorSetData
        {
            public readonly int headType;
            public readonly int bodyType;
            public readonly int legsType;
            public readonly string armorSetID;

            public ArmorSetData(int headType, int bodyType, int legsType, string armorSetID)
            {
                this.headType = headType;
                this.bodyType = bodyType;
                this.legsType = legsType;
                this.armorSetID = armorSetID;
            }
        }

        public static List<ArmorSetData> ArmorSets;

        internal static void Initialize()
        {
            ArmorSets = new List<ArmorSetData>
            {
                new ArmorSetData(ItemID.SpiderMask, ItemID.SpiderBreastplate, ItemID.SpiderGreaves, "SpiderArmor")
            };
        }

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            foreach (ArmorSetData data in ArmorSets.Where(
                data => data.headType == head.type && data.bodyType == body.type && data.legsType == legs.type))
                return data.armorSetID;

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            switch (set)
            {
                case "SpiderArmor":
                    if (CataclysmConfig.Instance.spiderArmorBuff)
                    {
                        player.setBonus += "\nYou can stick to walls like a spider";
                        player.spikedBoots = 3;
                    }
                    break;
            }
        }
    }
}
