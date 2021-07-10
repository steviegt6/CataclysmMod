using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.Default.Items;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Split;
using Split.Items.Accesories.Expert;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.Items.Accessories
{
    [ModDependency("Split")]
    public class PharaohsFear : CataclysmItem
    {
        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            DisplayName.SetDefault("Pharaoh's Fear");
            Tooltip.SetDefault("Grants immunity to knockback and fire blocks" +
                               "\nGrants immunity to most debuffs" +
                               "\nTemporarily increases defense shortly after being struck" +
                               "\nReleases homing skulls when moving" +
                               "\nTheir damage scales with defense");

            // todo: funny reference in tooltip probably idk lol
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.Size = new Vector2(26f);
            item.value = Item.sellPrice(gold: 23, silver: 5);
            item.accessory = true;
            item.rare = ItemRarityID.Expert;
            item.defense = 4;
            item.expert = true;
            item.glowMask = (short) GlowMaskRepository.GlowMasks[nameof(PharaohsFear)];
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            base.UpdateAccessory(player, hideVisual);

            SplitPlayer splitPlayer = player.GetModPlayer<SplitPlayer>();
            splitPlayer.accPalladiumDefense = true;
            splitPlayer.accInsurgentEon = true;
            splitPlayer.vanityAccTerrorShield = !hideVisual; // todo: recolor
            splitPlayer.vanityAccInsurgentEon = !hideVisual;

            player.fireWalk = true;
            player.noKnockback = true;

            // todo: buff ids eventually
            player.buffImmune[46] = true;
            player.buffImmune[33] = true;
            player.buffImmune[36] = true;
            player.buffImmune[30] = true;
            player.buffImmune[20] = true;
            player.buffImmune[32] = true;
            player.buffImmune[31] = true;
            player.buffImmune[35] = true;
            player.buffImmune[23] = true;
            player.buffImmune[22] = true;
        }

        public override void AddRecipes()
        {
            base.AddRecipes();

            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<TerrorShield>());
            recipe.AddIngredient(ItemID.AnkhShield);
            recipe.AddTile(TileID.TinkerersWorkbench);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
