using CalamityMod.Items.Weapons.Rogue;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.Recipes;
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
        }
    }
}