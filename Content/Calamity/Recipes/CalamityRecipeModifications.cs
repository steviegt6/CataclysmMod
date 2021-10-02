using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CataclysmMod.Content.Default.Recipes;
using CataclysmMod.Core.ModCompatibility;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.Recipes
{
    [ModDependency("CalamityMod")]
    public class CalamityRecipeModifications : RecipeContainer
    {
        public override void ModifyRecipes(Mod mod)
        {
            new RecipeSearch().WithIngredients((ItemID.RedBrick, 5)).WithTiles(TileID.Anvils)
                .WithResult((ModContent.ItemType<ThrowingBrick>(), 15)).EditRecipe(
                    editor =>
                    {
                        editor.DeleteTile(TileID.Anvils);
                        editor.AddTile(TileID.WorkBenches);
                    });

            new RecipeSearch()
                .WithIngredients((ModContent.ItemType<Lumenite>(), 6), (ModContent.ItemType<RuinousSoul>(), 4),
                    (ModContent.ItemType<ExodiumClusterOre>(), 12), (ItemID.SniperScope, 1))
                .WithTiles(TileID.LunarCraftingStation).WithResult((ModContent.ItemType<HalleysInferno>(), 1))
                .AsExactSearch().EditRecipe(
                    editor =>
                    {
                        editor.DeleteIngredient(ItemID.SniperScope);
                        editor.AddIngredient(ItemID.RifleScope);
                    });
        }
    }
}