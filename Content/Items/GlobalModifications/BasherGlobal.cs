using CalamityMod.Items.Weapons.Melee;
using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class BasherGlobal : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (CalamityChangesConfig.Instance.basherScale && item.type == ModContent.ItemType<Basher>())
                item.scale = 1.2f;
        }
    }
}