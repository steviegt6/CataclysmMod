using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
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
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;
using MemoryStream = System.IO.MemoryStream;

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

        public bool DepsLoaded;

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
            if (DepsLoaded)
                return;

            DepsLoaded = true;

            TmodFile tModFile = DirectDependencyReflection.ModFileProperty.GetValue(this) as TmodFile;
            Dictionary<string, TmodFile.FileEntry> files =
                (Dictionary<string, TmodFile.FileEntry>) DirectDependencyReflection.TmodFileFilesField.GetValue(
                    tModFile);

            IEnumerable<KeyValuePair<string, TmodFile.FileEntry>> directDependencyNames =
                files.Where(s => s.Key.StartsWith("lib") && s.Key.Contains("Direct") && s.Key.EndsWith(".dll"));

            foreach (KeyValuePair<string, TmodFile.FileEntry> kvp in directDependencyNames)
            {
                Logger.Debug($"Loading direct dependency: {kvp.Key}");

                Assembly asm = null;

                try
                {
                    if (Platform.IsWindows && !Environment.Is64BitProcess)
                    {
                        using (Stream stream = GetFileStream(kvp.Key))
                        using (MemoryStream mem = new MemoryStream())
                        {
                            stream.CopyTo(mem);
                            asm = Assembly.Load(mem.ToArray());
                        }
                    }
                    else
                        asm = DirectDependencyFallback(null,
                            new ResolveEventArgs(kvp.Key.Replace(".dll", "")));
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

                Type module = asm.GetTypes().FirstOrDefault(x => x.IsSubclassOf(typeof(DirectDependency)));

                if (module == null)
                {
                    Logger.Warn("No types extending DirectDependency found, proceeding to panic-log:");

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
        private Assembly DirectDependencyFallback(object sender, ResolveEventArgs args)
        {
            if (!args.Name.Contains("Direct"))
                return null;

            string libPath = $"{args.Name}.dll";

            try
            {
                Logger.Debug($"Attempting to load and rewrite assembly: {args.Name}.");

                //if (!this.GetPropertyValue<Mod, TmodFile>("File").HasFile(libPath))
                //    throw new Exception($"No direct dependency file found: {args.Name}.dll");

                using (Stream assembly = GetFileStream(libPath))
                    return RewriteAssembly(assembly);

            }
            catch (Exception e)
            {
                throw new Exception("Could not manually resolve assembly: " + args.Name, e);
            }
        }

        // Rewrites assemblies.
        // Map XNA namespace references to FNA.
        private Assembly RewriteAssembly(Stream assemblyStream)
        {
            Logger.Debug("Rewriting an assembly...");

            using (MemoryStream memStream = new MemoryStream())
            {
                assemblyStream.CopyTo(memStream);
                memStream.Position = 0;

                AssemblyDefinition definition = AssemblyDefinition.ReadAssembly(memStream);
                ModuleDefinition module = definition.MainModule;

                Logger.Debug($"Rewriting with assembly definition: {definition.FullName}");

                if (!Platform.IsWindows || Environment.Is64BitProcess)
                {
                    List<string> removedReferences = new List<string>
                    {
                        "Microsoft.Xna.Framework",
                        "Microsoft.Xna.Framework.Graphics",
                    };

                    List<(ModuleDefinition, Assembly)> replacementScopes = new List<(ModuleDefinition, Assembly)>
                    {
                        (ModuleDefinition.ReadModule(Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                            "ModCompile", "FNA.dll")), typeof(Vector2).Assembly),
                        (ModuleDefinition.ReadModule(typeof(Main).Assembly.Location), typeof(Main).Assembly )
                    };

                    void RemoveModReference(string modName)
                    {
                        if (ModLoader.GetMod(modName) is null)
                            return;

                        Logger.Debug($"Registering assembly to remove: {modName}");
                        removedReferences.Add(modName);
                        Logger.Debug($"Registered assembly to remove: {modName}");
                    }

                    void AddModModule(string modName)
                    {
                        if (ModLoader.GetMod(modName) is null)
                            return;

                        Logger.Debug($"Registering module to add: {modName}");

                        using (Stream stream = ModLoader.GetMod(modName).GetFileStream($"{modName}.FNA.dll"))
                        using (MemoryStream memCopy = new MemoryStream())
                        {
                            stream.CopyTo(memCopy);
                            memCopy.Position = 0;

                            replacementScopes.Add((
                                ModuleDefinition.ReadModule(memCopy, new ReaderParameters(ReadingMode.Deferred)),
                                ModLoader.GetMod(modName).Code));
                        }

                        Logger.Debug($"Registered module to add: {modName}");
                    }

                    List<string> mods = new List<string>
                    {
                        "CataclysmMod",
                        "CalamityMod"
                    };

                    foreach (string mod in mods)
                    {
                        RemoveModReference(mod);
                        AddModModule(mod);
                    }

                    Dictionary<string, Assembly> typeAssemblies = new Dictionary<string, Assembly>();

                    for (int i = 0; i < module.AssemblyReferences.Count; i++)
                        if (removedReferences.Any(x => module.AssemblyReferences[i].Name == x))
                        {
                            module.AssemblyReferences.RemoveAt(i);
                            i--;
                        }

                    Logger.Debug("Removed bad assembly references.");

                    foreach ((ModuleDefinition scopeModule, Assembly scopeAssembly) in replacementScopes)
                    {
                        foreach (TypeDefinition type in scopeModule.GetTypes())
                        {
                            if (!type.IsPublic || type.Namespace.Contains('<'))
                                continue;

                            typeAssemblies[type.FullName] = scopeAssembly;
                        }
                    }

                    void ChangeTypeScope(TypeReference typeReference)
                    {
                        if (typeReference is null || typeReference.FullName.StartsWith("System."))
                            return;

                        if (!typeAssemblies.TryGetValue(typeReference.FullName, out Assembly typeAssembly))
                            return;

                        typeReference.Scope = AssemblyNameReference.Parse(typeAssembly.FullName);
                    }

                    foreach ((ModuleDefinition _, Assembly assembly) in replacementScopes)
                        module.AssemblyReferences.Add(AssemblyNameReference.Parse(assembly.FullName));

                    foreach (TypeReference type in module.GetTypeReferences().OrderBy(x => x.FullName))
                        ChangeTypeScope(type);

                    foreach (TypeDefinition type in module.GetTypes())
                    foreach (CustomAttributeArgument constructorArg in type.CustomAttributes.SelectMany(attribute =>
                        attribute.ConstructorArguments))
                        if (constructorArg.Value is TypeReference typeReference)
                            ChangeTypeScope(typeReference);

                    Logger.Debug("Changed type scopes.");
                }

                using (MemoryStream newAssemblyStream = new MemoryStream())
                using (MemoryStream symbolStream = new MemoryStream())
                {
                    definition.Write(newAssemblyStream, new WriterParameters
                    {
                        WriteSymbols = true,
                        SymbolStream = symbolStream,
                        SymbolWriterProvider = new PortablePdbWriterProvider()
                    });

                    Logger.Debug("Wrote rewritten assembly to stream.");

                    return Assembly.Load(newAssemblyStream.ToArray(), symbolStream.ToArray());
                }
            }
        }
    }
}