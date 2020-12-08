using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class SpiderArmorGlobal : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            switch (item.type)
            {
                case ItemID.SpiderMask:
                    item.defense += 3;
                    break;

                case ItemID.SpiderBreastplate:
                    item.defense += 2;
                    break;

                case ItemID.SpiderGreaves:
                    item.defense += 1;
                    break;
            }
        }
    }
}
