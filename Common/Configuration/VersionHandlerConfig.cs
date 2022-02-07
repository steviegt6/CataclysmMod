#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.ComponentModel;
using Newtonsoft.Json;

namespace CataclysmMod.Common.Configuration
{
    public sealed class VersionHandlerConfig : JsonConfig<VersionHandlerConfig>
    {
        [JsonProperty("lastLoadedVersion")]
        [DefaultValue("0.0.0.0")]
        public string LastLoadedVersion = "0.0.0.0";

        [JsonProperty("seenStartupScreen")]
        [DefaultValue(false)]
        public bool SeenStartupScreen;
    }
}