using CalamityMod.Items.Weapons.Rogue;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        public static CataclysmMod Instance { get; private set; }

        public CataclysmMod()
        {
            Instance = this;

            Properties = new ModProperties
            {
                Autoload = true,
                AutoloadBackgrounds = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load() => LoadIL();

        public override void Unload() => UnloadIL();

        public override void PostAddRecipes()
        {
            // Modify Throwing Brick recipe
            RecipeFinder finder = new RecipeFinder();
            finder.AddIngredient(ItemID.RedBrick, 5);
            finder.AddTile(TileID.Anvils);
            finder.SetResult(ModContent.ItemType<ThrowingBrick>(), 15);

            RecipeEditor editor = new RecipeEditor(finder.FindExactRecipe());
            editor.DeleteTile(TileID.Anvils);
            editor.AddTile(TileID.WorkBenches);
        }
    }
}