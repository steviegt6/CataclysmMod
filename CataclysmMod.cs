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

                if (dependencies.Length == 0)
                    break;

                foreach (ModDependencyAttribute dependency in dependencies)
                {
                    if (!ModLoader.Mods.Any(x => x.Name.Equals(dependency.Mod)))
                    {
                        modRecord.Add(dependency.Mod);
                        break;
                    }

                    string contentName = type.Name;
                    object content = Activator.CreateInstance(type);

                    if (content is CataclysmItem cataclysmItem)
                    {
                        cataclysmItem.Autoload(ref contentName);
                        
                        if (cataclysmItem.LoadWithValidMods()) 
                            AddItem(contentName, cataclysmItem);
                    }
                }
            }

            foreach (string mod in modRecord)
            {
                Logger.Warn($"Attempted to load content from mod: {mod}! This content was not loaded. If you want this content, enable the given mod.");
            }
        }
    }
}