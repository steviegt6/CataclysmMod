using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.ClickerClass;
using CataclysmMod.Content.Default.Configs;
using CataclysmMod.Content.Default.GlobalModifications;
using CataclysmMod.Content.Default.Items;
using CataclysmMod.Content.Default.MonoMod;
using CataclysmMod.Content.Default.Projectiles;
using CataclysmMod.Content.Default.Recipes;
using CataclysmMod.Core.ModCompatibility;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class Cataclysm : Mod
    {
        public static Action<Mod> PreAddRecipeHooks;
        public static Action<Mod> AddRecipeHooks;
        public static Action<Mod> PostAddRecipeHooks;
        public static Action<Mod> PreAddRecipeGroupHooks;
        public static Action<Mod> AddRecipeGroupHooks;
        public static Action<Mod> PostAddRecipeGroupHooks;
        public static Action<Mod> ModifyRecipes;

        public readonly List<string> ModRecord = new List<string>();

        public static List<Hook> Hooks { get; private set; }

        public static List<(MethodInfo, Delegate)> Modifiers { get; private set; }

        public bool LoadedDependencies;

        public Cataclysm()
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
            Hooks = new List<Hook>();
            Modifiers = new List<(MethodInfo, Delegate)>();

            AddConfig(nameof(CataclysmPersonalConfig), new CataclysmPersonalConfig());

            GlowMaskRepository.Load();
            ClickerCompatibilityCalls.Load();

            Logger.Debug("Loading mod-dependent content...");

            LoadModDependentContent();

            Logger.Debug("Loaded mod-dependent content.");
        }

        public override void Unload()
        {
            PreAddRecipeHooks = null;
            AddRecipeHooks = null;
            PostAddRecipeHooks = null;
            PreAddRecipeGroupHooks = null;
            AddRecipeGroupHooks = null;
            PostAddRecipeGroupHooks = null;
            ModifyRecipes = null;

            foreach (Hook hook in Hooks)
                hook.Undo();

            foreach ((MethodInfo method, Delegate @delegate) in Modifiers)
                HookEndpointManager.Unmodify(method, @delegate);

            Hooks = null;
            Modifiers = null;

            ClickerCompatibilityCalls.Unload();
            GlowMaskRepository.Unload();
        }

        public override void AddRecipes()
        {
            PreAddRecipeHooks?.Invoke(this);
            AddRecipeHooks?.Invoke(this);
            PostAddRecipeHooks?.Invoke(this);
        }

        public override void AddRecipeGroups()
        {
            PreAddRecipeHooks?.Invoke(this);
            AddRecipeGroupHooks?.Invoke(this);
            PostAddRecipeGroupHooks?.Invoke(this);
        }

        public override void PostAddRecipes()
        {
            ModifyRecipes?.Invoke(this);
        }

        public override void PostSetupContent()
        {
            foreach (string mod in ModRecord)
                Logger.Warn($"There was content in Cataclysm that depends on: {mod}!" +
                            "\nThis content was not loaded as the given mod is not enabled.");
        }

        private void LoadModDependentContent()
        {
            foreach (Type type in Code.GetTypes()
                .Where(x => !x.IsAbstract && x.GetConstructor(Type.EmptyTypes) != null))
            {
                // Logger.Debug(type.Name);

                ModDependencyAttribute[] dependencies = type.GetCustomAttributes<ModDependencyAttribute>().ToArray();
                bool missingDependency = false;

                if (dependencies.Length == 0) continue;

                foreach (ModDependencyAttribute dependency in dependencies)
                {
                    if (ModLoader.Mods.Any(x => x.Name.Equals(dependency.Mod)))
                        continue;

                    missingDependency = true;

                    if (!ModRecord.Contains(dependency.Mod))
                        ModRecord.Add(dependency.Mod);
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

                    case CataclysmProjectile cataclysmProjectile:
                        if (cataclysmProjectile.Autoload(ref contentName))
                            continue;

                        if (cataclysmProjectile.LoadWithValidMods())
                            AddProjectile(contentName, cataclysmProjectile);
                        break;

                    // special logic for rogue weapons and stuff
                    case ModItem modItem:
                        modItem.Autoload(ref contentName); // assumes always false :)
                        AddItem(contentName, modItem);
                        break;

                    case RecipeContainer recipeContainer:
                        PreAddRecipeHooks += recipeContainer.PreAddRecipes;
                        AddRecipeHooks += recipeContainer.AddRecipes;
                        PostAddRecipeHooks += recipeContainer.PostAddRecipes;

                        PreAddRecipeGroupHooks += recipeContainer.PreAddRecipeGroups;
                        AddRecipeGroupHooks += recipeContainer.AddRecipeGroups;
                        PostAddRecipeGroupHooks += recipeContainer.PostAddRecipeGroups;

                        ModifyRecipes += recipeContainer.ModifyRecipes;
                        break;

                    case CataclysmGlobalItem cataclysmGlobalItem:
                        if (cataclysmGlobalItem.Autoload(ref contentName))
                            continue;

                        if (cataclysmGlobalItem.LoadWithValidMods())
                            AddGlobalItem(contentName, cataclysmGlobalItem);
                        break;

                    case CataclysmGlobalNpc cataclysmGlobalNpc:
                        if (cataclysmGlobalNpc.Autoload(ref contentName))
                            continue;

                        if (cataclysmGlobalNpc.LoadWithValidMods())
                            AddGlobalNPC(contentName, cataclysmGlobalNpc);
                        break;

                    case CataclysmGlobalProjectile cataclysmGlobalProjectile:
                        if (cataclysmGlobalProjectile.Autoload(ref contentName))
                            continue;

                        if (cataclysmGlobalProjectile.LoadWithValidMods())
                            AddGlobalProjectile(contentName, cataclysmGlobalProjectile);
                        break;

                    case CataclysmPlayer cataclysmPlayer:
                        if (cataclysmPlayer.Autoload(ref contentName))
                            continue;

                        if (cataclysmPlayer.LoadWithValidMods())
                            AddPlayer(contentName, cataclysmPlayer);
                        break;

                    case MonoModPatcher<string> monoModPatcher:
                        monoModPatcher.Apply();
                        break;

                    case MonoModPatcher<MethodInfo> monoModPatcher:
                        monoModPatcher.Apply();
                        break;
                }
            }
        }
    }
}