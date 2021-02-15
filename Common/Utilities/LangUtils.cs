using Terraria.Localization;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class LangUtils
    {
        internal static string GetCalamityTextValue(string key) => GetModTextValue($"Calamity.{key}");

        internal static string GetModTextValue(string key) => GetModTextValue(CataclysmMod.Instance, key);

        internal static LocalizedText GetModText(string key) => GetModText(CataclysmMod.Instance, key);

        public static string GetModTextValue(Mod mod, string key) => Language.GetTextValue($"Mods.{mod.Name}.{key}");

        public static LocalizedText GetModText(Mod mod, string key) => Language.GetText($"Mods.{mod.Name}.{key}");
    }
}