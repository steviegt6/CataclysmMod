using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Content.Default.Configs
{
    public class CataclysmPersonalConfig : ModConfig
    {
        public static CataclysmPersonalConfig Instance { get; private set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded()
        {
            Instance = this;
        }

        [Header("Calamity")]
        [Label("Show Organic Enemy Status Text")]
        [DefaultValue(false)]
        public bool ShowOrganicText { get; set; }
    }
}