using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mono.Cecil;
using Mono.Cecil.Cil;
using MonoMod.RuntimeDetour;
using MonoMod.RuntimeDetour.HookGen;
using ReLogic.OS;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

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
            AppDomain.CurrentDomain.AssemblyResolve += DirectDependencyFallback;

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
            AppDomain.CurrentDomain.AssemblyResolve -= DirectDependencyFallback;

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
                Logger.Warn($"There was content in Cataclysm that depends on: {mod}!" +
                            $"\nThis content was not loaded as the given mod is not enabled.");
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
            TmodFile tModFile = DirectDependencyReflection.ModFileProperty.GetValue(this) as TmodFile;
            Dictionary<string, TmodFile.FileEntry> files =
                (Dictionary<string, TmodFile.FileEntry>) DirectDependencyReflection.TmodFileFilesField.GetValue(
                    tModFile);

            IEnumerable<KeyValuePair<string, TmodFile.FileEntry>> directDependencyNames = files.Where(s =>
                s.Key.StartsWith("lib") && s.Key.Contains("CataclysmMod.Direct") && s.Key.EndsWith(".dll"));

            foreach (KeyValuePair<string, TmodFile.FileEntry> kvp in directDependencyNames)
            {
                Logger.Debug($"Loading direct dependency: {kvp.Key}");
                
                Assembly asm = null;

                try
                {
                    asm = ReferenceDirectDependency(kvp.Key);
                }
                catch
                {
                    Logger.Warn("Failed to load direct dependency's assembly.");
                }

                if (asm is null)
                {
                    Logger.Warn("Loaded assembly was null.");
                    continue;
                }

                Logger.Debug($"Loaded direct dependency assembly: {asm.GetName().Name}, FullName: {asm.FullName}");

                Type module = asm.GetTypes().FirstOrDefault(x => x.FullName?.EndsWith("Main") ?? false);

                if (module == null)
                {
                    Logger.Warn("Type ROOT.Main was null, proceeding to panic-log:");

                    Logger.Debug("Listing all assemblies in the app domain:");

                    foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                        Logger.Debug($"Found assembly name: {assembly.GetName().Name}, FullName: {assembly.FullName}");

                    Logger.Debug("Listing all reflection-only assemblies in the app domain:");

                    foreach (Assembly assembly in AppDomain.CurrentDomain.ReflectionOnlyGetAssemblies())
                        Logger.Debug(
                            $"Found reflection-only assembly name: {assembly.GetName().Name}, FullName: {assembly.FullName}");

                    Logger.Debug("Panic-logging complete, attempting to move forward...");

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

        // We hook into AppDomain.CurrentDomain.AssemblyResolve to use this.
        // *Nix & 64bit will fail to resolve our DirectXDependency assemblies.
        // Here, we can load them manually and perform rewrites for compatibility.
        private static Assembly DirectDependencyFallback(object sender, ResolveEventArgs args)
        {
            AssemblyName name = new AssemblyName(args.Name);

            if (!name.Name.Contains("CataclysmMod.Direct"))
                return null;

            Cataclysm mod = ModContent.GetInstance<Cataclysm>();

            if (mod is null)
                throw new Exception("Cataclysm not yet loaded.");

            string libPath = $"lib/{name.Name}.dll";

            try
            {
                if (!mod.GetPropertyValue<Mod, TmodFile>("File").HasFile(libPath))
                    throw new Exception($"No direct dependency file found: {name.Name}.dll");

                using (Stream assembly = mod.GetFileStream(libPath))
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    assembly.CopyTo(memoryStream);
                    return RewriteAssembly(memoryStream);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Could not manually resolve assembly: " + args.Name, e);
            }
        }

        // Rewrites assemblies.
        // Map XNA namespace references to FNA.
        private static Assembly RewriteAssembly(Stream assemblyStream)
        {
            AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(assemblyStream);
            ModuleDefinition module = definition.MainModule;

            if (!Platform.IsWindows)
            {
                List<Assembly> addedReferences = new List<Assembly>
                {
                    typeof(Vector2).Assembly,
                    typeof(SpriteBatch).Assembly
                };

                List<string> removedReferences = new List<string>
                {
                    "Microsoft.Xna.Framework",
                    "Microsoft.Xna.Framework.Graphics",
                };

                Dictionary<Assembly, AssemblyNameReference> targetReferences = addedReferences.ToDictionary(x => x,
                    x => AssemblyNameReference.Parse(x.FullName));

                Dictionary<Assembly, ModuleDefinition> targetModules = addedReferences.ToDictionary(x => x,
                    x => ModuleDefinition.ReadModule(x.Modules.Single().FullyQualifiedName,
                        new ReaderParameters {InMemory = true}));


                Dictionary<string, Assembly> typeAssemblies = new Dictionary<string, Assembly>();

                for (int i = 0; i < module.AssemblyReferences.Count; i++)
                    if (removedReferences.Any(x => module.AssemblyReferences[i].Name == x))
                    {
                        module.AssemblyReferences.RemoveAt(i);
                        i--;
                    }

                foreach (Assembly assembly in addedReferences)
                {
                    ModuleDefinition scopeModule = targetModules[assembly];

                    foreach (TypeDefinition type in scopeModule.GetTypes())
                    {
                        if (!type.IsPublic || type.Namespace.Contains('<'))
                            continue;

                        typeAssemblies[type.FullName] = assembly;
                    }
                }

                void ChangeTypeScope(TypeReference typeReference)
                {
                    if (typeReference is null || typeReference.FullName.StartsWith("System."))
                        return;

                    if (!typeAssemblies.TryGetValue(typeReference.FullName, out Assembly typeAssembly))
                        return;

                    typeReference.Scope = targetReferences[typeAssembly];
                }

                foreach (AssemblyNameReference target in targetReferences.Values)
                    module.AssemblyReferences.Add(target);

                foreach (TypeReference type in module.GetTypeReferences().OrderBy(x => x.FullName))
                    ChangeTypeScope(type);

                foreach (TypeDefinition type in module.GetTypes())
                foreach (CustomAttributeArgument constructorArg in type.CustomAttributes.SelectMany(attribute =>
                    attribute.ConstructorArguments))
                    if (constructorArg.Value is TypeReference typeReference)
                        ChangeTypeScope(typeReference);
            }

            using (MemoryStream newAssemblyStream = new MemoryStream())
            using (MemoryStream symbolStream = new MemoryStream())
            {
                definition.Write(newAssemblyStream, new WriterParameters
                {
                    WriteSymbols = true,
                    SymbolStream = symbolStream,
                    SymbolWriterProvider = new DefaultSymbolWriterProvider()
                });

                return Assembly.Load(newAssemblyStream.ToArray());
            }
        }

        private static Assembly ReferenceDirectDependency(string dependency)
        {
            switch (dependency)
            {
                case "lib/CataclysmMod.DirectCalamityDependencies.dll":
                    return Calamity();
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private static Assembly Calamity() => typeof(CataclysmMod.DirectCalamityDependencies.Main).Assembly;
    }
}