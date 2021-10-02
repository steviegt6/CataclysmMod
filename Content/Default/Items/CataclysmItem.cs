using CataclysmMod.Core.ModCompatibility;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Default.Items
{
    public abstract class CataclysmItem : ModItem, IModDependent
    {
        public override string Texture =>
            ModContent.TextureExists(base.Texture) ? base.Texture : "ModLoader/MysteryItem";

        public virtual bool LoadWithValidMods() => true;

        public virtual bool DependsOnMod() => true;

        public sealed override bool Autoload(ref string name)
        {
            ModifyContentName(ref name);
            return !DependsOnMod();
        }

        public virtual void ModifyContentName(ref string name)
        {
        }
    }
}