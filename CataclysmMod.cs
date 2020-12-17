using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CataclysmMod.Utilities;
using Terraria;
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

            if (finder.TryFindExactRecipe(out RecipeEditor throwingBrick))
            {
                throwingBrick.DeleteTile(TileID.Anvils);
                throwingBrick.AddTile(TileID.WorkBenches);
            }

            // Modify Halley's Inferno recipe
            finder = new RecipeFinder();
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