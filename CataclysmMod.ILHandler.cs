using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        internal void LoadIL()
        {
            //LoadDetours();

            IL.CalamityMod.World.SmallBiomes.PlaceShrines += ChangeCavernShrineBlocks;
            IL.CalamityMod.World.WorldGenerationMethods.SpecialChest += ChanceCavernShrineChest;
            IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory += RemoveSummonDamageBonus;
            IL.Terraria.ItemText.NewText += ChangeItemTextColor;
            IL.Terraria.ItemText.UpdateItemText += UpdateAnimatedCalamityRarities;
            IL.Terraria.Main.MouseText += MouseTextRarityColors;
            IL.Terraria.Item.Prefix += RemovePrefixRarityCap;
        }

        internal void UnloadIL()
        {
            IL.CalamityMod.World.SmallBiomes.PlaceShrines -= ChangeCavernShrineBlocks;
            IL.CalamityMod.World.WorldGenerationMethods.SpecialChest -= ChanceCavernShrineChest;
            IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory -= RemoveSummonDamageBonus;
            IL.Terraria.ItemText.NewText -= ChangeItemTextColor;
            IL.Terraria.ItemText.UpdateItemText -= UpdateAnimatedCalamityRarities;
            IL.Terraria.Main.MouseText -= MouseTextRarityColors;
            IL.Terraria.Item.Prefix -= RemovePrefixRarityCap;
        }
    }
}
