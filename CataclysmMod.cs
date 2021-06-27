using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.Items;
using CataclysmMod.Content.Default.Recipes;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public static Action<Mod> PreAddRecipeHooks;
        public static Action<Mod> AddRecipeHooks;
        public static Action<Mod> PostAddRecipeHooks;
        public static Action<Mod> PreAddRecipeGroupHooks;
        public static Action<Mod> AddRecipeGroupHooks;
        public static Action<Mod> PostAddRecipeGroupHooks;

        public CataclysmMod()
        {
            Properties = new ModProperties
            {
                Autoload = false,
                AutoloadBackgrounds = false,
                AutoloadGores = false,
                AutoloadSounds = false
            };
        }

        public override void Load()
        {
            Logger.Debug("Loading mod-dependent content...");

            LoadModDependentContent();

            Logger.Debug("Loaded mod-dependent content.");
        }

        public override void AddRecipes()
        {
            PreAddRecipeHooks?.Invoke(this);
            AddRecipeHooks?.Invoke(this);
            PostAddRecipeHooks?.Invoke(this);
        }

        public override void PostAddRecipes()
        {
            PreAddRecipeHooks?.Invoke(this);
            AddRecipeGroupHooks?.Invoke(this);
            PostAddRecipeGroupHooks?.Invoke(this);
        }

        private void LoadModDependentContent()
        {
            List<string> modRecord = new List<string>();

            foreach (Type type in Code.GetTypes().Where(x => !x.IsAbstract && x.GetConstructor(new Type[0]) != null))
            {
                ModDependencyAttribute[] dependencies = type.GetCustomAttributes<ModDependencyAttribute>().ToArray();
                bool missingDependency = false;

                if (dependencies.Length == 0) continue;

                foreach (ModDependencyAttribute dependency in dependencies)
                {
                    if (ModLoader.Mods.Any(x => x.Name.Equals(dependency.Mod)) ||
                        modRecord.Contains(dependency.Mod)) continue;

                    missingDependency = true;
                    modRecord.Add(dependency.Mod);
                }

                if (missingDependency) continue;

                string contentName = type.Name;
                object content = Activator.CreateInstance(type);

                switch (content)
                {
                    case CataclysmItem cataclysmItem:
                        if (cataclysmItem.Autoload(ref contentName))
                            continue;

                        if (cataclysmItem.LoadWithValidMods())
                            AddItem(contentName, cataclysmItem);
                        break;

                    case RecipeContainer recipeContainer:
                        PreAddRecipeHooks += recipeContainer.PreAddRecipes;
                        AddRecipeHooks += recipeContainer.AddRecipes;
                        PostAddRecipeHooks += recipeContainer.PostAddRecipes;

                        PreAddRecipeGroupHooks += recipeContainer.PreAddRecipeGroups;
                        AddRecipeGroupHooks += recipeContainer.AddRecipeGroups;
                        PostAddRecipeGroupHooks += recipeContainer.PostAddRecipeGroups;
                        break;
                }
            }

            foreach (string mod in modRecord)
            {
                Logger.Warn(
                    $"There was content in Cataclysm that depends on: {mod}! This content was not loaded as the given mod is not enabled.");
            }
        }
    }
}