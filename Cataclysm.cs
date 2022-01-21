using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public sealed class Cataclysm : Mod
    {
        public Dictionary<Type, Addon> RegisteredAddons = new Dictionary<Type, Addon>
        {
            {typeof(CalamityModAddon), CalamityModAddon.Instance},
            {typeof(ClickerClassAddon), ClickerClassAddon.Instance},
            {typeof(SplitAddon), SplitAddon.Instance},
            {typeof(ThoriumModAddon), ThoriumModAddon.Instance}
        };

        public Cataclysm()
        {
            // Disable autoloading entirely. We will autoload everything.
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
            base.Load();

            Logger.Info("Registered addons:");

            foreach (Addon addon in RegisteredAddons.Values)
                Logger.Info($"  {addon.InternalName}: minimum v{addon.MinimumVersion}");

            Logger.Debug("Loading addon content...");

            Autoload();
        }

        private void Autoload()
        {
            IOrderedEnumerable<Type> types = Code.GetTypes().OrderBy(
                type => type.FullName,
                StringComparer.InvariantCulture
            );

            foreach (Type type in types)
            {
                if (type.IsAbstract || type.GetConstructor(Type.EmptyTypes) == null)
                    continue;

                AddonContentAttribute attribute = type.GetCustomAttribute<AddonContentAttribute>();

                if (attribute != null)
                    if (!RegisteredAddons[attribute.AddonType].IsEnabled)
                        continue;

                object instance = Activator.CreateInstance(type);
                string name = type.Name;

                if (type.IsSubclassOf(typeof(ModItem)))
                    AddItem(name, (ModItem) instance);
                //else if (type.IsSubclassOf(typeof(GlobalItem)))
                //    AddGlobalItem(type);
                //else if (type.IsSubclassOf(typeof(ModPrefix)))
                //    AddPrefix(type);
                //else if (type.IsSubclassOf(typeof(ModDust)))
                //    AddDust(type);
                //else if (type.IsSubclassOf(typeof(ModTile)))
                //    AddTile(type);
                //else if (type.IsSubclassOf(typeof(GlobalTile)))
                //    AddGlobalTile(type);
                //else if (type.IsSubclassOf(typeof(ModTileEntity)))
                //    AddTileEntity(type);
                //else if (type.IsSubclassOf(typeof(ModWall)))
                //    AddWall(type);
                //else if (type.IsSubclassOf(typeof(GlobalWall)))
                //    AddGlobalWall(type);
                //else if (type.IsSubclassOf(typeof(ModProjectile)))
                //    AddProjectile(type);
                //else if (type.IsSubclassOf(typeof(GlobalProjectile)))
                //    AddGlobalProjectile(type);
                //else if (type.IsSubclassOf(typeof(ModNPC)))
                //    AddNPC(type);
                //else if (type.IsSubclassOf(typeof(GlobalNPC)))
                //    AddGlobalNPC(type);
                //else if (type.IsSubclassOf(typeof(ModPlayer)))
                //    AddPlayer(type);
                //else if (type.IsSubclassOf(typeof(ModBuff)))
                //    AddBuff(type);
                //else if (type.IsSubclassOf(typeof(GlobalBuff)))
                //    AddGlobalBuff(type);
                //else if (type.IsSubclassOf(typeof(ModMountData)))
                //    AddMount(type);
                //else if (type.IsSubclassOf(typeof(ModGore)))
                //    modGores.Add(type);
                //else if (type.IsSubclassOf(typeof(ModSound)))
                //    modSounds.Add(type);
                //else if (type.IsSubclassOf(typeof(ModWorld)))
                //    AddModWorld(type);
                //else if (type.IsSubclassOf(typeof(ModUgBgStyle)))
                //    AddUgBgStyle(type);
                //else if (type.IsSubclassOf(typeof(ModSurfaceBgStyle)))
                //    AddSurfaceBgStyle(type);
                //else if (type.IsSubclassOf(typeof(GlobalBgStyle)))
                //    AddGlobalBgStyle(type);
                //else if (type.IsSubclassOf(typeof(ModWaterStyle)))
                //    AddWaterStyle(type);
                //else if (type.IsSubclassOf(typeof(ModWaterfallStyle)))
                //    AddWaterfallStyle(type);
                //else if (type.IsSubclassOf(typeof(GlobalRecipe)))
                //    AddGlobalRecipe(type);
                //else if (type.IsSubclassOf(typeof(ModCommand)))
                //    AddCommand(type);
            }
        }
    }
}