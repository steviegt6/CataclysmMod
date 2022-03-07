#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using CalamityMod.Items.Materials;
using CalamityMod.Items.Placeables.Ores;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CataclysmMod.Common.Configuration.ModConfigs;
using CataclysmMod.Common.Recipes;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Localization;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Addons
{
    public class CalamityModAddon : Addon<CalamityModAddon>
    {
        public override string InternalName => "CalamityMod";
        
        public override string DisplayName => "Calamity Mod";

        public override Version MinimumVersion => new Version(1, 5, 0, 4);

        public override ModConfig Config => ModContent.GetInstance<CalamityModAddonConfig>();

        public override string Description => FilelessEntries.GetCalamityDescription();

        public override void LoadEnabled()
        {
            base.LoadEnabled();

            ModContent.GetInstance<Cataclysm>().ModifyRecipes += () =>
            {
                new RecipeModifier()
                    .WithIngredients((ItemID.RedBrick, 5))
                    .WithTiles(TileID.Anvils)
                    .WithResult((ModContent.ItemType<ThrowingBrick>(), 15))
                    .EditRecipe(e =>
                    {
                        e.DeleteTile(TileID.Anvils);
                        e.AddTile(TileID.WorkBenches);
                    });

                new RecipeModifier()
                    .WithIngredients(
                        (ModContent.ItemType<Lumenite>(), 6),
                        (ModContent.ItemType<RuinousSoul>(), 4),
                        (ModContent.ItemType<ExodiumClusterOre>(), 12),
                        (ItemID.SniperScope, 1)
                    )
                    .WithTiles(TileID.LunarCraftingStation)
                    .WithResult((ModContent.ItemType<HalleysInferno>(), 1))
                    .WithExactSearch()
                    .EditRecipe(
                        editor =>
                        {
                            editor.DeleteIngredient(ItemID.SniperScope);
                            editor.AddIngredient(ItemID.RifleScope);
                        });
            };
        }
    }
}