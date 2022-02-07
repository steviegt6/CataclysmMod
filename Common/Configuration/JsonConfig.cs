#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.IO;
using Newtonsoft.Json;
using Terraria;

namespace CataclysmMod.Common.Configuration
{
    public abstract class JsonConfig<TConfig> where TConfig : JsonConfig<TConfig>, new()
    {
        public static string Path => System.IO.Path.Combine(
            Main.SavePath,
            "Cataclysm",
            typeof(TConfig).Name.ToLower() + ".json"
        );

        public static void SerializeConfig(TConfig config) => File.WriteAllText(
            Path,
            JsonConvert.SerializeObject(config)
        );

        public static TConfig DeserializeConfig() => !File.Exists(Path)
            ? new TConfig()
            : JsonConvert.DeserializeObject<TConfig>(File.ReadAllText(Path));
    }
}