using Terraria.ModLoader;

namespace CataclysmMod.Content.Items
{
    public abstract class CalamityCompatItem : ModItem
    {
        public override bool Autoload(ref string name) => CataclysmMod.Instance.Calamity != null;
    }
}