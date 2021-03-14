﻿using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Content.Configs
{
    [Label("Cataclysm Config")]
    public class CataclysmConfig : ModConfig
    {
        [Label("Abyss Mines From the Slime God Explode")]
        [Tooltip("Abyss mines now all explode once the Slime God is defeated.")]
        [DefaultValue(true)]
        public bool abyssMinesExplode;

        [Label("Increase Angry Dog Spawn Rate & Cryophobia Drop Chance")] [DefaultValue(true)]
        public bool angryDogSpawnBuff;

        // NPC Changes
        [Header("NPC Changes")]
        [Label("Anomura Fungus Spawning")]
        [Tooltip("Anomura Fungus can now spawn in underground mushroom biomes in pre-HM")]
        [DefaultValue(true)]
        public bool anomuraFungusSpawning;

        [Label("Basher Buff")] [Tooltip("Basher now has 1.2x scale.")] [DefaultValue(true)]
        public bool basherScale;

        // General Changes
        [Header("General Changes")]
        [Label("Cavern Shrine Block Changes")]
        [Tooltip("Cavern Shrine now uses gray bricks and stone instead of obsidian.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool cavernShrineChanges;

        [Label("Dagger of Decree")]
        [Tooltip("Add the Dagger of Decree, a rare variant of the Cursed Dagger.")]
        [DefaultValue(true)]
        public bool daggerOfDecree;

        [Label("Specify Whether an NPC is Organic or Inorganic")]
        [Tooltip("Display text indicating if an NPC is organic or not under their healthbar.")]
        [DefaultValue(false)]
        public bool displayOrganicTextNPCs;

        [Label("Drataliornus Arrows")]
        [Tooltip("Drataliornus Arrows can now be shot through blocks.")]
        [DefaultValue(true)]
        public bool drataliornusArrowsThroughBlocks;

        [Label("Fungal Clump Emits Light")] [DefaultValue(true)]
        public bool fungalClumpEmitsLight;

        [Label("Grand Shark Repellent")]
        [Tooltip("Add the Grand Shark Repellent, an accessory which stands the Grand Sand Shark from spawning.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool grandSharkRepellent;

        [Label("Halley's Inferno Recipe Change")]
        [Tooltip("Halley's Inferno uses a rifle scope instead of a sniper scope.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool halleysInfernoRecipeChange;

        [Label("Infinity (weapon) Never Consumes Ammo")] [DefaultValue(true)]
        public bool infinityDontConsumeAmmo;

        // Item Changes
        [Header("Item Changes")]
        [Label("Lore Items in Piggy Banks")]
        [Tooltip("Lore items' effects will be applied if in the player's piggy bank.")]
        [DefaultValue(true)]
        public bool loreItemsInPiggyBank;

        [Label("More NPCs Drop Shark Fins")] [DefaultValue(true)]
        public bool npcsDropSharkFins = true;

        [Label("Obsidian Skull + Upgrades Provide Heat Immunity")] [DefaultValue(true)]
        public bool obsidianSkullHeatImmunity;

        [Label("Pickaxe Tooltips")]
        [Tooltip("Pickaxes now have consistent tooltips for what ores they can mine.")]
        [DefaultValue(true)]
        // TODO: Make pickaxe code for this not garbage.
        public bool pickaxeTooltips;

        [Label("Traveling Merchant Has a 10% Chance to Drop Pulse Bow in Hardmode")] [DefaultValue(true)]
        public bool pulseBowDrop;

        [Label("Slime God Inflicts Slimed Instead of Distorted")] [DefaultValue(true)]
        public bool slimeGodSlimedDebuff;

        // Projectile Changes
        [Header("Projectile Changes")]
        [Label("Improved Minion Rotation")]
        [Tooltip(
            "Give minions (ice clasper, corvid harbringer, herring staff, cinder blossom staff, and calamari lament's minions more interesting rotation.")]
        [DefaultValue(true)]
        public bool smootherMinionRotation;

        [Label("Spider Armor Buff")]
        [Tooltip("Spider Armor defense buffed and allows you to cling to walls.")]
        [DefaultValue(true)]
        public bool spiderArmorBuff;

        [Label("Steampunker Spawn Fix")]
        [Tooltip("Steampunker no longer announces that is \"has awoken\" when spawning.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool steampunkerSpawnFix;

        [Label("Sulphurous Shell")]
        [Tooltip("Add the Sulphurous Shell, which teleports you to the Sulphurous Sea.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool sulphurousShell;

        [Label("Sulphurskin Potion Sell Price Nerf")]
        [Tooltip("Reduce the sell price of Sulphurskin Potions so you can't easily sell them to make tons of money.")]
        [DefaultValue(true)]
        public bool sulphurSkinPotionPriceNerf;

        [Label("Throwing Bricks Recipe Change")]
        [Tooltip("Throwing Bricks are now crafted at a work bench instead of an anvil.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool throwingBrickRecipeChange;

        [Label("Torrential Tear Death Mode Nerf Removal")]
        [Tooltip("Removes Torrential Tear's Death mode nerf as it's absolutely pointless and just hinders vision.")]
        [ReloadRequired]
        [DefaultValue(true)]
        public bool torrentialTearNerfRemoval;

        [Label("Voodoo Dolls Stack to 20")] [DefaultValue(true)]
        public bool voodooDollStackIncrease;

        [Label("Wizard Sells Guide Voodoo Dolls")] [DefaultValue(true)]
        public bool wizardGuideVoodooDoll;

        public static CataclysmConfig Instance { get; internal set; }

        public override ConfigScope Mode => ConfigScope.ClientSide;

        public override void OnLoaded() => Instance = this;
    }
}
