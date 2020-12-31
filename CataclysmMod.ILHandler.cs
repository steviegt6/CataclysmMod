using CataclysmMod.Common.Configs;
using CataclysmMod.Common.Detours;
using CataclysmMod.Common.IL;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        internal void LoadIL()
        {
            // IL to change the appearance of Cavern Shrines
            if (CalamityChangesConfig.Instance.cavernShrineChanges)
            {
                IL.CalamityMod.World.SmallBiomes.PlaceShrines += CavernShrine.ChangeCavernShrineBlocks;
                IL.CalamityMod.World.WorldGenerationMethods.SpecialChest += CavernShrine.ChangeCavernShrineChest;
            }

            // IL to change Fungal Clump's damage
            if (CalamityChangesConfig.Instance.fungalClumpTrueDamage)
                IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory += FungalClumpDamage.RemoveSummonDamageBonus;

            if (CalamityChangesConfig.Instance.steampunkerSpawnFix)
                IL.CalamityMod.World.CalamityWorld.PostUpdate += SteampunkerSpawnIL.ModifySteampunkerSpawn;

            /* Detours */
            // Remove annoying Death mode changes to the Torrential Tear
            if (CalamityChangesConfig.Instance.torrentialTearNerfRemoval)
                On.CalamityMod.Items.Tools.ClimateChange.TorrentialTear.UseItem += TorrentialTearDetour.RemoveDeathModeCrap;
        }

        internal void UnloadIL()
        {
            if (CalamityChangesConfig.Instance.cavernShrineChanges)
            {
                IL.CalamityMod.World.SmallBiomes.PlaceShrines -= CavernShrine.ChangeCavernShrineBlocks;
                IL.CalamityMod.World.WorldGenerationMethods.SpecialChest -= CavernShrine.ChangeCavernShrineChest;
            }

            if (CalamityChangesConfig.Instance.fungalClumpTrueDamage)
                IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory -= FungalClumpDamage.RemoveSummonDamageBonus;

            if (CalamityChangesConfig.Instance.steampunkerSpawnFix)
                IL.CalamityMod.World.CalamityWorld.PostUpdate -= SteampunkerSpawnIL.ModifySteampunkerSpawn;
        }
    }
}