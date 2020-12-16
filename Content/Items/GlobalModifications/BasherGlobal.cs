using CalamityMod.Items.Weapons.Melee;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class BasherGlobal : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;
        }
    }
}