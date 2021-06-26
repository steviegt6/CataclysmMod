using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Items;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
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

        private void LoadModDependentContent()
        {
            List<string> modRecord = new List<string>();

            foreach (Type type in Code.GetTypes().Where(x => !x.IsAbstract && x.GetConstructor(new Type[0]) != null))
            {
                ModDependencyAttribute[] dependencies = type.GetCustomAttributes<ModDependencyAttribute>().ToArray();
                bool missingDependency = false;

                if (dependencies.Length == 0)
                    continue;

                foreach (ModDependencyAttribute dependency in dependencies)
                {
                    if (!ModLoader.Mods.Any(x => x.Name.Equals(dependency.Mod)) && !modRecord.Contains(dependency.Mod))
                    {
                        missingDependency = true;
                        modRecord.Add(dependency.Mod);
                    }
                }

                if (missingDependency)
                    continue;

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