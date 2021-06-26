using CataclysmMod.Common.ModCompatibility;

namespace CataclysmMod.Content.Default.Items.Tests
{
    [ModDependency("ThoriumMod")]
    public class ThoriumItemDependency : CataclysmItem
    {
        public override bool LoadWithValidMods() => false;
    }
}