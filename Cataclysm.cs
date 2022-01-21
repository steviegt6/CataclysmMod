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
                    NamelessAddItem((ModItem) instance, name);
                //else if (type.IsSubclassOf(typeof(GlobalItem)))
                //    NamelessAddGlobalItem(type);
                //else if (type.IsSubclassOf(typeof(ModPrefix)))
                //    NamelessAddPrefix(type);
                //else if (type.IsSubclassOf(typeof(ModDust)))
                //    NamelessAddDust(type);
                //else if (type.IsSubclassOf(typeof(ModTile)))
                //    NamelessAddTile(type);
                //else if (type.IsSubclassOf(typeof(GlobalTile)))
                //    NamelessAddGlobalTile(type);
                //else if (type.IsSubclassOf(typeof(ModTileEntity)))
                //    NamelessAddTileEntity(type);
                //else if (type.IsSubclassOf(typeof(ModWall)))
                //    NamelessAddWall(type);
                //else if (type.IsSubclassOf(typeof(GlobalWall)))
                //    NamelessAddGlobalWall(type);
                //else if (type.IsSubclassOf(typeof(ModProjectile)))
                //    NamelessAddProjectile(type);
                //else if (type.IsSubclassOf(typeof(GlobalProjectile)))
                //    NamelessAddGlobalProjectile(type);
                //else if (type.IsSubclassOf(typeof(ModNPC)))
                //    NamelessAddNPC(type);
                //else if (type.IsSubclassOf(typeof(GlobalNPC)))
                //    NamelessAddGlobalNPC(type);
                //else if (type.IsSubclassOf(typeof(ModPlayer)))
                //    NamelessAddPlayer(type);
                //else if (type.IsSubclassOf(typeof(ModBuff)))
                //    NamelessAddBuff(type);
                //else if (type.IsSubclassOf(typeof(GlobalBuff)))
                //    NamelessAddGlobalBuff(type);
                //else if (type.IsSubclassOf(typeof(ModMountData)))
                //    NamelessAddMount(type);
                //else if (type.IsSubclassOf(typeof(ModGore)))
                //    modGores.NamelessAdd(type);
                //else if (type.IsSubclassOf(typeof(ModSound)))
                //    modSounds.NamelessAdd(type);
                //else if (type.IsSubclassOf(typeof(ModWorld)))
                //    NamelessAddModWorld(type);
                //else if (type.IsSubclassOf(typeof(ModUgBgStyle)))
                //    NamelessAddUgBgStyle(type);
                //else if (type.IsSubclassOf(typeof(ModSurfaceBgStyle)))
                //    NamelessAddSurfaceBgStyle(type);
                //else if (type.IsSubclassOf(typeof(GlobalBgStyle)))
                //    NamelessAddGlobalBgStyle(type);
                //else if (type.IsSubclassOf(typeof(ModWaterStyle)))
                //    NamelessAddWaterStyle(type);
                //else if (type.IsSubclassOf(typeof(ModWaterfallStyle)))
                //    NamelessAddWaterfallStyle(type);
                //else if (type.IsSubclassOf(typeof(GlobalRecipe)))
                //    NamelessAddGlobalRecipe(type);
                //else if (type.IsSubclassOf(typeof(ModCommand)))
                //    NamelessAddCommand(type);
            }
        }

        private void NamelessAddItem(ModItem item, string name)
        {
            if (item.Autoload(ref name))
                AddItem(name, item);
        }
    }
}