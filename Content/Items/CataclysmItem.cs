using Terraria.ModLoader;

namespace CataclysmMod.Content.Items
{
    public abstract class CataclysmItem : ModItem
    {
        public override string Texture
        {
            get
            {
                if (ModContent.TextureExists(base.Texture))
                    return base.Texture;

                mod.Logger.Warn($"Texture for item: {Name} - not found! Falling back to a placeholder texture!");
                return "ModLoaderMod/MysteryItem";
            }
        }
    }
}