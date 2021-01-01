using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Common.Configs
{
    [Label("Calamity Changes")]
    public class CalamityChangesConfig : ModConfig
    {
        public static CalamityChangesConfig Instance { get; internal set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        // General Changes
        [Header("General Changes")]
        [Label("Cavern Shrine Block Changes")]
        [Tooltip("Cavern Shrine now uses gray bricks and stone instead of obsidian.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool cavernShrineChanges;

        // Item Changes
        [Header("Item Changes")]
        [Label("Lore Items in Piggy Banks")]
        [Tooltip("Lore items' effects will be applied if in the player's piggy bank.")]
        [DefaultValue(true)]
        public bool loreItemsInPiggyBank;

        [Label("Grand Shark Repellent")]
        [Tooltip("Add the Grand Shark Repellent, an accessory which stands the Grand Sand Shark from spawning.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool grandSharkRepellent;

        [Label("Fungal Clump Deals True Damage")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool fungalClumpTrueDamage;

        [Label("Sulphurous Shell")]
        [Tooltip("Add the Sulphurous Shell, which teleports you to the Sulphurous Sea.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool sulphurousShell;

        [Label("Spider Armor Buff")]
        [Tooltip("Spider Armor defense buffed and allows you to cling to walls.")]
        [DefaultValue(true)]
        public bool spiderArmorBuff;

        [Label("Drataliornus Arrows")]
        [Tooltip("Drataliornus Arrows can now be shot through blocks.")]
        [DefaultValue(true)]
        public bool drataliornusArrowsThroughBlocks;

        [Label("Basher Buff")]
        [Tooltip("Basher now has 1.2x scale.")]
        [DefaultValue(true)]
        public bool basherScale;

        [Label("Pickaxe Tooltips")]
        [Tooltip("Pickaxes now have consistent tooltips for what ores they can mine.")]
        [DefaultValue(true)] // TODO: Make pickaxe code for this not garbage.
        public bool pickaxeTooltips;

        [Label("Throwing Bricks Recipe Change")]
        [Tooltip("Throwing Bricks are now crafted at a work bench instead of an anvil.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool throwingBrickRecipeChange;

        [Label("Halley's Infero Recipe Change")]
        [Tooltip("Halley's Inferno uses a rifle scope instead of a sniper scope.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool halleysInfernoRecipeChange;

        [Label("Torrential Tear Death Mode Nerf Removal")]
        [Tooltip("Removes Torrential Tear's Death mode nerf as it's absolutely pointless and just hinders vision.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool torrentialTearNerfRemoval;

        [Label("Sulphurskin Potion Sell Price Nerf")]
        [Tooltip("Reduce the sell price of Sulphurskin Potions so you can't easily sell them to make tons of money.")]
        [DefaultValue(true)]
        public bool sulphurSkinPotionPriceNerf;

        [Label("Dagger of Decree")]
        [Tooltip("Add the Dagger of Decree, a rare variant of the Cursed Dagger.")]
        [DefaultValue(true)]
        public bool daggerOfDecree;

        // NPC Changes
        [Header("NPC Changes")]
        [Label("Anomura Fungus Spawning")]
        [Tooltip("Anomura Fungus can now spawn in underground mushroom biomes in pre-HM")]
        [DefaultValue(true)]
        public bool anomuraFungusSpawning;

        [Label("Steampunker Spawn Fix")]
        [Tooltip("Steampunker no longer announces that is \"has awoken\" when spawning.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool steampunkerSpawnFix;

        [Label("Wizard Sells Guide Voodoo Dools")]
        [DefaultValue(true)]
        public bool wizardGuideVoodooDoll;

        [Label("Abyss Mines From the Slime God Explode")]
        [Tooltip("Abyss mines now all explode once the Slime God is defeated.")]
        [DefaultValue(true)]
        public bool abyssMinesExplode;

        [Label("Increase Angry Dog Spawn Rate & Cryophobia Drop Chance")]
        [DefaultValue(true)]
        public bool angryDogSpawnBuff;

        // Projectile Changes
        [Header("Projectile Changes")]
        [Label("Improved Minion Rotation")]
        [Tooltip("Give minions (ice clasper, corvid harbringer, herring staff, cinder blossom staff, and calamari lament's minions more interesting rotation.")]
        [DefaultValue(true)]
        public bool smootherMinionRotation;
    }
}