using System;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Exceptions
{
    internal sealed class CalamityVersionException : Exception
    {
        internal CalamityVersionException(Version calamityVersion, Version expectedVersion, ExceptionType outdatedType)
        {
            CalamityVersion = calamityVersion;
            ExpectedVersion = expectedVersion;
            OutdatedType = outdatedType;
        }

        public Version CalamityVersion { get; }

        public Version ExpectedVersion { get; }

        public ExceptionType OutdatedType { get; }

        public override string Message
        {
            get
            {
                switch (OutdatedType)
                {
                    case ExceptionType.OutdatedCalamity:
                        return
                            $"Your version of Calamity ({CalamityVersion}) does not match the expected version of {ExpectedVersion}!" +
                            "\nYou can download a newer version of Calamity on the browser or through the provided Help Link!";

                    case ExceptionType.OutdatedCataclysm:
                        return
                            $"Your version of Cataclysm ({CataclysmMod.Instance.Version}) is outdated and does not support the loaded version of Calamity {CalamityVersion}!" +
                            $"\nThis version of Cataclysm only supports Calamity {CataclysmMod.ExpectedCalamityVersion}." +
                            "\nBe sure to check if there is an update available for Cataclysm either through the Mod Browser or through the Help Link provided.";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public override string HelpLink
        {
            get
            {
                switch (OutdatedType)
                {
                    case ExceptionType.OutdatedCalamity:
                        return "https://mirror.sgkoi.dev/tModLoader/download.php?Down=mods/CalamityMod.tmod";

                    case ExceptionType.OutdatedCataclysm:
                        return "https://mirror.sgkoi.dev/tModLoader/download.php?Down=mods/CataclysmMod.tmod";

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public static void ThrowErrorOnIncorrectVersion(Mod calamityMod, Version expectedVersion)
        {
            Version calamityVersion = calamityMod.Version;

            // Throw an error if the loaded Calamity mod is outdated
            if (calamityVersion < expectedVersion)
                throw new CalamityVersionException(calamityVersion, CataclysmMod.ExpectedCalamityVersion,
                    ExceptionType.OutdatedCalamity);

            // Throw an error if the version of Calamity is more recent than what was expected
            if (CataclysmMod.ExpectedCalamityVersion < calamityVersion)
                throw new CalamityVersionException(calamityVersion, null, ExceptionType.OutdatedCataclysm);
        }

        internal enum ExceptionType
        {
            OutdatedCalamity,
            OutdatedCataclysm
        }
    }
}