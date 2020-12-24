using CataclysmMod.Common.IL;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        internal void LoadIL()
        {
            // IL to change the appearance of Cavern Shrines
            IL.CalamityMod.World.SmallBiomes.PlaceShrines += CavernShrine.ChangeCavernShrineBlocks;
            IL.CalamityMod.World.WorldGenerationMethods.SpecialChest += CavernShrine.ChangeCavernShrineChest;

            // IL to change Fungal Clump's damage
            IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory += FungalClumpDamage.RemoveSummonDamageBonus;

            IL.CalamityMod.World.CalamityWorld.PostUpdate += SteampunkerSpawnIL.ModifySteampunkerSpawn;
        }

        internal void UnloadIL()
        {
            IL.CalamityMod.World.SmallBiomes.PlaceShrines -= CavernShrine.ChangeCavernShrineBlocks;
            IL.CalamityMod.World.WorldGenerationMethods.SpecialChest -= CavernShrine.ChangeCavernShrineChest;

            IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory -= FungalClumpDamage.RemoveSummonDamageBonus;

            IL.CalamityMod.World.CalamityWorld.PostUpdate -= SteampunkerSpawnIL.ModifySteampunkerSpawn;
        }
    }
}