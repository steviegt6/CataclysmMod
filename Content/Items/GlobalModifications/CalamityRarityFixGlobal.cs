using CalamityMod;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.GlobalModifications
{
    public class CalamityRarityFixGlobal : GlobalItem
    {
        public override bool PreDrawInInventory(Item item, SpriteBatch spriteBatch, Vector2 position, Rectangle frame, Color drawColor, Color itemColor, Vector2 origin, float scale)
        {
            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);

            item.rare = item.Calamity().customRarity != CalamityRarity.NoEffect ? (int)item.Calamity().customRarity : item.rare;

            if (item.Calamity().customRarity == CalamityRarity.ItemSpecific)
                item.rare = int.Parse($"{10000}{item.type}");

            return base.PreDrawInInventory(item, spriteBatch, position, frame, drawColor, itemColor, origin, scale);
        }
    }
}