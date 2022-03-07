#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Recipes
{
    /// <summary>
    ///     Builder-based recipe modification utility.
    /// </summary>
    public class RecipeModifier
    {
        public List<(int, int)> Ingredients = new List<(int, int)>();
        public List<int> Tiles = new List<int>();
        public (int, int) Result = (0, 0);
        public bool IsExact;

        public RecipeModifier WithIngredients(params (int, int)[] ingredients)
        {
            Ingredients.AddRange(ingredients);
            return this;
        }

        public RecipeModifier WithTiles(params int[] tiles)
        {
            Tiles.AddRange(tiles);
            return this;
        }

        public RecipeModifier WithResult((int, int) result)
        {
            Result = result;
            return this;
        }

        public RecipeModifier WithExactSearch(bool exact = true)
        {
            IsExact = exact;
            return this;
        }

        public void EditRecipe(Action<RecipeEditor> editAction)
        {
            RecipeFinder finder = new RecipeFinder();
            Ingredients.ForEach(x => finder.AddIngredient(x.Item1, x.Item2));
            Tiles.ForEach(x => finder.AddTile(x));

            if (Result.Item1 != 0)
                finder.SetResult(Result.Item1, Result.Item2);

            try
            {
                if (IsExact)
                {
                    Recipe rec = finder.FindExactRecipe();

                    if (rec is null)
                    {
                        ModContent.GetInstance<Cataclysm>().Logger.Error("An exact recipe could not be matched!");
                        return;
                    }
                    
                    editAction?.Invoke(new RecipeEditor(rec));
                }
                else
                    foreach (Recipe recipe in finder.SearchRecipes())
                        editAction?.Invoke(new RecipeEditor(recipe));
            }
            catch (Exception e)
            {
                ModContent.GetInstance<Cataclysm>().Logger.Warn($"Exception during recipe editing thrown: {e}");
            }
        }
    }
}