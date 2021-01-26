using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CataclysmMod.Common.Configs;
using CataclysmMod.Utilities;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Recipes
{
    public class CalamityCompatRecipes : ModCompatRecipe
    {
        public override bool Autoload() => CataclysmMod.Instance.Calamity != null;

        public override void ModifyRecipes()
        {
            if (CalamityChangesConfig.Instance.throwingBrickRecipeChange)
            {
                RecipeFinder finder = new RecipeFinder();
                finder.AddIngredient(ItemID.RedBrick, 5);
                finder.AddTile(TileID.Anvils);
                finder.SetResult(ModContent.ItemType<ThrowingBrick>(), 15);

                if (finder.TryFindExactRecipe(out RecipeEditor throwingBrick))
                {
                    throwingBrick.DeleteTile(TileID.Anvils);
                    throwingBrick.AddTile(TileID.WorkBenches);
                }
            }

            if (CalamityChangesConfig.Instance.halleysInfernoRecipeChange)
            {
                RecipeFinder finder = new RecipeFinder();
                finder.AddIngredient(ModContent.ItemType<Lumenite>(), 6);
                finder.AddIngredient(ModContent.ItemType<RuinousSoul>(), 4);
                finder.AddIngredient(ModContent.ItemType<ExodiumClusterOre>(), 12);
                finder.AddIngredient(ItemID.SniperScope);
                finder.AddTile(TileID.LunarCraftingStation);
                finder.SetResult(ModContent.ItemType<HalleysInferno>());

                if (finder.TryFindExactRecipe(out RecipeEditor halleysInferno))
                {
                    halleysInferno.DeleteIngredient(ItemID.SniperScope);
                    halleysInferno.AddIngredient(ItemID.RifleScope);
                }
            }
        }
    }
}