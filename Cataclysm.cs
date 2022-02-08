using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CataclysmMod.Common.Addons;
using CataclysmMod.Common.Configuration;
using CataclysmMod.Common.UserInterface;
using CataclysmMod.Common.UserInterface.AddonDisplay;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public sealed class Cataclysm : Mod
    {
        public VersionHandlerConfig VhConfig = new VersionHandlerConfig();

        public UIAddons AddonsUI;
        public UIAddonInfo AddonInfoUI;
        
        public readonly UserInterfaceHandler InterfaceHandler = new UserInterfaceHandler();
        public readonly Dictionary<Type, Addon> RegisteredAddons = new Dictionary<Type, Addon>
        {
            {typeof(CalamityModAddon), CalamityModAddon.Instance},
            {typeof(ClickerClassAddon), ClickerClassAddon.Instance},
            {typeof(SplitAddon), SplitAddon.Instance},
            {typeof(ThoriumModAddon), ThoriumModAddon.Instance}
        };

        public bool WarningsShown = false;

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

            VhConfig = VersionHandlerConfig.DeserializeConfig();

            Logger.Info("Registered addons:");

            foreach (Addon addon in RegisteredAddons.Values)
                Logger.Info($"  {addon.InternalName}: minimum v{addon.MinimumVersion}");
            
            Logger.Debug("Loading addon content...");

            Autoload();

            AddonsUI = new UIAddons();
            AddonInfoUI = new UIAddonInfo();
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
                while (Main.menuMode != 0)
                {
                }

                Main.menuMode = 888;
                Main.MenuUI.SetState(AddonsUI);
            });
        }

        public override void UpdateUI(GameTime gameTime)
        {
            base.UpdateUI(gameTime);

            InterfaceHandler.UpdateStates(gameTime);
        }

        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            base.PostDrawInterface(spriteBatch);

            InterfaceHandler.DrawStates(spriteBatch);
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