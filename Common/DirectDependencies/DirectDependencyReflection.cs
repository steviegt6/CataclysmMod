using System;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CataclysmMod.Common.DirectDependencies
{
    public static class DirectDependencyReflection
    {
        public static Type ModType;
        public static PropertyInfo Mod_File;

        public static Type TmodFileType;
        public static FieldInfo TmodFile_files;

        public static void Load()
        {
            ModType = typeof(Mod);
            Mod_File = ModType.GetProperty("File", BindingFlags.NonPublic | BindingFlags.Instance);

            TmodFileType = typeof(TmodFile);
            TmodFile_files = TmodFileType.GetField("files", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void Unload()
        {
            ModType = null;
            Mod_File = null;

            TmodFileType = null;
            TmodFile_files = null;
        }
    }
}
