using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CataclysmMod.Common.Addons;
using CataclysmMod.Common.Configuration;
using CataclysmMod.Common.UserInterface.AddonDisplay;
using CataclysmMod.Core.Loading;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod
{
    public sealed class Cataclysm : Mod
    {
        public VersionHandlerConfig VhConfig = new VersionHandlerConfig();

        public UIAddons AddonsUI;
        public UIAddonInfo AddonInfoUI;
        public bool ShowChangelog;
        public bool FirstTime;
        
        public readonly Dictionary<Type, Addon> RegisteredAddons = new Dictionary<Type, Addon>
        {
            {typeof(CalamityModAddon), CalamityModAddon.Instance},
            {typeof(ClickerClassAddon), ClickerClassAddon.Instance},
            {typeof(SplitAddon), SplitAddon.Instance},
            {typeof(ThoriumModAddon), ThoriumModAddon.Instance},
            {typeof(AutoloadAddon), AutoloadAddon.Instance}
        };

        public Cataclysm()
        {
            // Disable autoloading regular content.
            // Keep gores, backgrounds, and sounds around.
            Properties = new ModProperties
            {
                Autoload = false,
                AutoloadBackgrounds = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public override void Load()
        {
            base.Load();

            MonoModHooks.RequestNativeAccess();
            
            VhConfig = VersionHandlerConfig.DeserializeConfig();

            Logger.Info("Registered addons:");

            foreach (Addon addon in RegisteredAddons.Values)
                Logger.Info($"  {addon.InternalName}: minimum v{addon.MinimumVersion}");
            
            Logger.Debug("Loading addon content...");

            Autoload();

            AddonsUI = new UIAddons();
            AddonInfoUI = new UIAddonInfo();

            ShowChangelog = Version.Parse(VhConfig.LastLoadedVersion) < Version;
            VhConfig.LastLoadedVersion = Version.ToString();
        }

        public override void Unload()
        {
            base.Unload();
            
            VersionHandlerConfig.SerializeConfig(VhConfig);
        }

        public override void PostAddRecipes()
        {
            base.PostAddRecipes();

            new TaskFactory().StartNew(() =>
            {
                while (Main.menuMode != 0 && ShowChangelog)
                {
                }

                Main.menuMode = 888;
                Main.MenuUI.SetState(new UIChangelog(0, GetChangelog()));
            });
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
                else if (type.IsSubclassOf(typeof(GlobalItem)))
                    AddGlobalItem(name, (GlobalItem) instance);
                else if (type.IsSubclassOf(typeof(ModPrefix)))
                    AddPrefix(name, (ModPrefix) instance);
                else if (type.IsSubclassOf(typeof(ModDust)))
                    AddDust(name, (ModDust) instance);
                else if (type.IsSubclassOf(typeof(ModTile)))
                    AddTile(name, (ModTile) instance, ((IHasTexture) instance).LoadedTexture);
                else if (type.IsSubclassOf(typeof(GlobalTile)))
                    AddGlobalTile(name, (GlobalTile) instance);
                else if (type.IsSubclassOf(typeof(ModTileEntity)))
                    AddTileEntity(name, (ModTileEntity) instance);
                else if (type.IsSubclassOf(typeof(ModWall)))
                    AddWall(name, (ModWall) instance, ((IHasTexture) instance).LoadedTexture);
                else if (type.IsSubclassOf(typeof(GlobalWall)))
                    AddGlobalWall(name, (GlobalWall) instance);
                else if (type.IsSubclassOf(typeof(ModProjectile)))
                    AddProjectile(name, (ModProjectile) instance);
                else if (type.IsSubclassOf(typeof(GlobalProjectile)))
                    AddGlobalProjectile(name, (GlobalProjectile) instance);
                else if (type.IsSubclassOf(typeof(ModNPC)))
                    AddNPC(name, (ModNPC) instance);
                else if (type.IsSubclassOf(typeof(GlobalNPC)))
                    AddGlobalNPC(name, (GlobalNPC) instance);
                else if (type.IsSubclassOf(typeof(ModPlayer)))
                    AddPlayer(name, (ModPlayer) instance);
                else if (type.IsSubclassOf(typeof(ModBuff)))
                    AddBuff(name, (ModBuff) instance, ((IHasTexture) instance).LoadedTexture);
                else if (type.IsSubclassOf(typeof(GlobalBuff)))
                    AddGlobalBuff(name, (GlobalBuff) instance);
                // else if (type.IsSubclassOf(typeof(ModMountData)))
                //     AddMount(name, (ModMountData) instance);
                // else if (type.IsSubclassOf(typeof(ModGore)))
                //     modGores.Add(name, (ModGore) instance);
                // else if (type.IsSubclassOf(typeof(ModSound)))
                //     modSounds.Add(name, (ModSound) instance);
                else if (type.IsSubclassOf(typeof(ModWorld)))
                    AddModWorld(name, (ModWorld) instance);
                // else if (type.IsSubclassOf(typeof(ModUgBgStyle)))
                //     AddUgBgStyle(name, (ModUgBgStyle) instance);
                // else if (type.IsSubclassOf(typeof(ModSurfaceBgStyle)))
                //     AddSurfaceBgStyle(name, (ModSurfaceBgStyle) instance);
                // else if (type.IsSubclassOf(typeof(GlobalBgStyle)))
                //     AddGlobalBgStyle(name, (GlobalBgStyle) instance);
                // else if (type.IsSubclassOf(typeof(ModWaterStyle)))
                //     AddWaterStyle(name, (ModWaterStyle) instance);
                else if (type.IsSubclassOf(typeof(ModWaterfallStyle)))
                    AddWaterfallStyle(name, (ModWaterfallStyle) instance, ((IHasTexture) instance).LoadedTexture);
                else if (type.IsSubclassOf(typeof(GlobalRecipe)))
                    AddGlobalRecipe(name, (GlobalRecipe) instance);
                else if (type.IsSubclassOf(typeof(ModCommand)))
                    AddCommand(name, (ModCommand) instance);
                else if (type.IsSubclassOf(typeof(ModConfig)))
                    AddConfig(name, (ModConfig) instance);
                else if (instance is ILoadable loadable)
                    loadable.Load();
            }
        }

        private string GetChangelog()
        {
            string text = VhConfig.SeenStartupScreen ? "" : TextValue("Changelog.Startup");
            text += $"\n{TextValue("Changelog.ActualChangelog")}";

            VhConfig.SeenStartupScreen = true;

            return text;
        }

        public static LocalizedText Text(string key) => LanguageManager.Instance.GetText("Mods.CataclysmMod." + key);
        
        public static string TextValue(string key, params object[] args) => Text(key).Format(args);
    }
}