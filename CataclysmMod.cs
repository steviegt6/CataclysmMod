using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.DirectDependencies;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Common.Utilities;
using CataclysmMod.Content.ClickerClass;
using CataclysmMod.Content.Default.Configs;
using CataclysmMod.Content.Default.GlobalModifications;
using CataclysmMod.Content.Default.Items;
using CataclysmMod.Content.Default.MonoMod;
using CataclysmMod.Content.Default.Projectiles;
using CataclysmMod.Content.Default.Recipes;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.OS;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

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
        public static Action<Mod> ModifyRecipes;

        public readonly List<string> ModRecord = new List<string>();

        public static List<Hook> Hooks { get; private set; }

        public static List<(MethodInfo, Delegate)> Modifiers { get; private set; }

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
            Hooks = new List<Hook>();
            Modifiers = new List<(MethodInfo, Delegate)>();

            AddConfig(nameof(CataclysmPersonalConfig), new CataclysmPersonalConfig());

            GlowMaskRepository.Load();
            ClickerCompatibilityCalls.Load();

            Logger.Debug("Loading mod-dependent content...");

            LoadModDependentContent();

            Logger.Debug("Preparing direct dependency content...");

            DirectDependencyReflection.Load();

            Logger.Debug("Loading direct dependency content...");

            LoadDirectDependencies();

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

            CataclysmPersonalConfig.Instance = null;

            foreach (Hook hook in Hooks)
                hook.Undo();

            foreach ((MethodInfo method, Delegate @delegate) in Modifiers)
                HookEndpointManager.Unmodify(method, @delegate);

            Hooks = null;
            Modifiers = null;

            DirectDependencyReflection.Unload();
            ClickerCompatibilityCalls.Unload();
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
                Logger.Warn(
                    $"There was content in Cataclysm that depends on: {mod}! This content was not loaded as the given mod is not enabled.");
        }

        private void LoadModDependentContent()
        {
            foreach (Type type in Code.GetTypes().Where(x => !x.IsAbstract && x.GetConstructor(new Type[0]) != null))
            {
                // Logger.Debug(type.Name);

                ModDependencyAttribute[] dependencies = type.GetCustomAttributes<ModDependencyAttribute>().ToArray();
                bool missingDependency = false;

                if (dependencies.Length == 0) continue;

                foreach (ModDependencyAttribute dependency in dependencies)
                {
                    if (ModLoader.Mods.Any(x => x.Name.Equals(dependency.Mod))) continue;

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

        private void LoadDirectDependencies()
        {
            bool FnaFromPlatform(string s) => Platform.IsWindows ? !s.Contains("FNA") : s.Contains("FNA");

            TmodFile tModFile = DirectDependencyReflection.ModFileProperty.GetValue(this) as TmodFile;
            Dictionary<string, TmodFile.FileEntry> files =
                (Dictionary<string, TmodFile.FileEntry>) DirectDependencyReflection.TmodFileFilesField.GetValue(
                    tModFile);

            IEnumerable<KeyValuePair<string, TmodFile.FileEntry>> directDependencyNames =
                files.Where(s =>
                    s.Key.StartsWith("lib") && s.Key.Contains("CataclysmMod.Direct") && s.Key.EndsWith(".dll") &&
                    FnaFromPlatform(s.Key));

            foreach (KeyValuePair<string, TmodFile.FileEntry> kvp in directDependencyNames)
            {
                Logger.Debug($"Loading library: {kvp.Key}");

                byte[] dllBytes = GetFileBytes(kvp.Key);
                Assembly asm = null;

                try
                {
                    asm = Assembly.Load(dllBytes);
                }
                catch
                {
                    Logger.Warn("Failed to load assembly.");
                }

                if (asm is null)
                {
                    Logger.Warn("Loaded assembly was null.");
                    continue;
                }

                Logger.Debug($"Loaded assembly name: {asm.GetName().Name}, FullName: {asm.FullName}");

                Type module = asm.GetType("ROOT.Main");

                if (module == null)
                {
                    Logger.Warn("Type ROOT.Main was null.");

                    Logger.Debug("Listing all assemblies in the app domain:");

                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        Logger.Debug($"Found assembly name: {assembly.GetName().Name}, FullName: {assembly.FullName}");

                    Logger.Debug("Listing all reflection-only assemblies in the app domain:");

                    foreach (Assembly assembly in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
                        Logger.Debug($"Found reflection-only assembly name: {assembly.GetName().Name}, FullName: {assembly.FullName}");

                    /*try
                    Logger.Debug("Listing all located assembly types:");

                    {
                        foreach (Type type in asm.GetTypes())
                            Logger.Debug($"Found type: {type.Name}");

                        if (asm.GetTypes().Length == 0)
                            Logger.Debug("Zero types in assembly found.");
                    }
                    catch (ReflectionTypeLoadException e)
                    {
                        Logger.Error($"ReflectionTypeLoadException thrown: {e}");

                        Logger.Debug("Underlying exceptions:");

                        foreach (Exception exception in e.LoaderExceptions)
                        {
                            Logger.Debug(exception.ToString());
                        }
                    }*/

                    continue;
                }

                bool missingMods = false;

                if (!(Activator.CreateInstance(module) is DirectDependency instance))
                    continue;

                foreach (string mod in instance.DependsOn)
                {
                    if (!(ModLoader.GetMod(mod) is null))
                        continue;

                    missingMods = true;

                    if (!ModRecord.Contains(mod))
                        ModRecord.Add(mod);
                }

                if (!missingMods)
                    instance.AddContent(this);
            }
        }
    }
}