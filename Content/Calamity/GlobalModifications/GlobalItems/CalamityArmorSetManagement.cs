using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Common.ArmorSets;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Terraria;
using Terraria.ID;

// TODO: move to default namespace and make it no longer associated with calamity?
namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalItems
{
    [ModDependency("CalamityMod")]
    public class CalamityArmorSetManagement : CataclysmGlobalItem
    {
        public List<ArmorSet> ArmorSets => new List<ArmorSet>
        {
            new ArmorSet(ItemID.SpiderMask, ItemID.SpiderBreastplate, ItemID.SpiderGreaves, "SpiderArmor")
        };

        public override string IsArmorSet(Item head, Item body, Item legs)
        {
            foreach (ArmorSet armorSet in ArmorSets.Where(x =>
                x.HeadType.Equals(head.type) && x.BodyType.Equals(body.type) && x.LegsType.Equals(legs.type)))
                return armorSet.SetId;

            return base.IsArmorSet(head, body, legs);
        }

        public override void UpdateArmorSet(Player player, string set)
        {
            switch (set)
            {
                case "SpiderArmor":
                    player.setBonus += "\nYou can stick to walls like a spider";
                    player.spikedBoots = 3;
                    break;
            }
        }
    }
}