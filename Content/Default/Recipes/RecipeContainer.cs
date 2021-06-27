using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Default.Recipes
{
    public abstract class RecipeContainer // : IModDependent
    {
        public class RecipeSearch
        {
            public List<(int, int)> Ingredients;
            public List<int> Tiles;
            public (int, int) Result;
            public bool IsExact;

            public RecipeSearch()
            {
                Ingredients = new List<(int, int)>();
                Tiles = new List<int>();
                Result = (0, 0);
                IsExact = false;
            }

            public RecipeSearch WithIngredients(params (int, int)[] ingredients)
            {
                Ingredients.AddRange(ingredients);
                return this;
            }

            public RecipeSearch WithTiles(params int[] tiles)
            {
                Tiles.AddRange(tiles);
                return this;
            }

            public RecipeSearch WithResult((int, int) result)
            {
                Result = result;
                return this;
            }

            public RecipeSearch AsExactSearch()
            {
                IsExact = true;
                return this;
            }

            public void EditRecipe(Action<RecipeEditor> editAction)
            {
                RecipeFinder finder = new RecipeFinder();
                Ingredients.ForEach(x => finder.AddIngredient(x.Item1, x.Item2));
                Tiles.ForEach(x => finder.AddTile(x));

                if (Result.Item1 != 0)
                    finder.SetResult(Result.Item1, Result.Item2);

                if (IsExact)
                    editAction?.Invoke(new RecipeEditor(finder.FindExactRecipe()));
                else
                    foreach (Recipe recipe in finder.SearchRecipes())
                        editAction?.Invoke(new RecipeEditor(recipe));
            }
        }

        public virtual void PreAddRecipes(Mod mod)
        {
        }

        public virtual void AddRecipes(Mod mod)
        {
        }

        public virtual void PostAddRecipes(Mod mod)
        {
        }

        public virtual void PreAddRecipeGroups(Mod mod)
        {
        }

        public virtual void AddRecipeGroups(Mod mod)
        {
        }

        public virtual void PostAddRecipeGroups(Mod mod)
        {
        }

        public virtual void ModifyRecipes(Mod mod)
        {
        }
    }
}