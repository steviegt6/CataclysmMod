using System;
using System.Reflection;
using CataclysmMod.Common.Utilities;
using Terraria.ModLoader;
using Terraria.ModLoader.Core;

namespace CataclysmMod.Common.DirectDependencies
{
    public static class DirectDependencyReflection
    {
        public static Type ModType;
        public static PropertyInfo ModFileProperty;

        public static Type TmodFileType;
        public static FieldInfo TmodFileFilesField;

        public static void Load()
        {
            ModType = typeof(Mod);
            ModFileProperty = ModType.GetCachedProperty("File");

            TmodFileType = typeof(TmodFile);
            TmodFileFilesField = TmodFileType.GetCachedField("files");
        }

        public static void Unload()
        {
            ModType = null;
            ModFileProperty = null;

            TmodFileType = null;
            TmodFileFilesField = null;
        }
    }
}