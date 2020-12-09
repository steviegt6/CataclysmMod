using CalamityMod;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        internal void LoadDetours()
        {
            On.Terraria.Item.Prefix += UncapRarities;
        }

        private bool UncapRarities(On.Terraria.Item.orig_Prefix orig, Item self, int pre)
        {
            bool returnValue = orig(self, pre);

            self.rare = self.Calamity().customRarity != CalamityRarity.NoEffect ? (int)self.Calamity().customRarity : self.rare;

            if (self.Calamity().customRarity == CalamityRarity.ItemSpecific)
                self.rare = int.Parse($"{10000}{self.type}");

            return returnValue;
        }
    }
}
