#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Common.Addons;
using CataclysmMod.Content.Vanilla.Components.Items;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Split;
using Split.Items.Accesories.Expert;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.Items.Accessories
{
    [AddonContent(typeof(SplitAddon))]
    public class PharaohsFear : ModItem, IGlowmaskedItemComponent
    {
        string IGlowmaskedItemComponent.GlowmaskPath => Texture + "_Glow";
        
        public override void SetDefaults()
        {
            base.SetDefaults();

            item.Size = new Vector2(26f);
            item.value = Item.sellPrice(gold: 23, silver: 5);
            item.accessory = true;
            item.rare = ItemRarityID.Expert;
            item.defense = 4;
            item.expert = true;
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