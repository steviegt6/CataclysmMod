using CataclysmMod.Common.Addons;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public sealed class Cataclysm : Mod
    {
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
            
            Logger.Debug(CalamityModAddon.Instance.InternalName);
            Logger.Debug(ClickerClassAddon.Instance.InternalName);
            Logger.Debug(SplitAddon.Instance.InternalName);
            Logger.Debug(ThoriumModAddon.Instance.InternalName);
        }
    }
}