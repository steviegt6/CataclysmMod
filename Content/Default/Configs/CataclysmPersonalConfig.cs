using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Content.Default.Configs
{
    public class CataclysmPersonalConfig : ModConfig
    {
        public static CataclysmPersonalConfig Instance => ModContent.GetInstance<CataclysmPersonalConfig>();

        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("Calamity")]
        [Label("Show Organic Enemy Status Text")]
        [DefaultValue(false)]
        public bool ShowOrganicText { get; set; }
    }
}