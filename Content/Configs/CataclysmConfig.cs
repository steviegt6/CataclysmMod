using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Content.Configs
{
    [Label("Cataclysm Config")]
    public class CataclysmConfig : ModConfig
    {
        public static CataclysmConfig Instance { get; internal set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded() => Instance = this;

        // General Changes
        [Header("General Changes")]
        [Label("Cavern Shrine Block Changes")]
        [Tooltip("Cavern Shrine now uses gray bricks and stone instead of obsidian.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool cavernShrineChanges;

        // Item Changes
        [Header("Item Changes")]
        [Label("Torrential Tear Death Mode Nerf Removal")]
        [Tooltip("Removes Torrential Tear's Death mode nerf as it's absolutely pointless and just hinders vision.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool torrentialTearNerfRemoval;

        // NPC Changes
        [Header("NPC Changes")]
        [Label("Specify Whether an NPC is Organic or Inorganic")]
        [Tooltip("Display text indicating if an NPC is organic or not under their healthbar.")]
        [DefaultValue(false)]
        public bool displayOrganicTextNPCs;

        [Label("Slime God Inflicts Slimed Instead of Distorted")]
        [DefaultValue(true)]
        public bool slimeGodSlimedDebuff;

        // Projectile Changes
        [Header("Projectile Changes")]
        [Label("Drataliornus Arrows Pass Through Blocks")]
        [Tooltip("Drataliornus Arrows can now be shot through blocks.")]
        [DefaultValue(true)]
        public bool drataliornusArrowsThroughBlocks;
    }
}