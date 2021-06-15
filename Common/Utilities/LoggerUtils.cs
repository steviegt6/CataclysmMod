namespace CataclysmMod.Common.Utilities
{
    public static class LoggerUtils
    {
        public static void LogPatchError(string opCode, string toMatch) =>
            CataclysmMod.Instance.Logger.Error($"[IL] Unable to match \"{opCode}\" \"{toMatch}\"!");

        public static void LogPatchError(string opCode, string toMatch, int index) =>
            CataclysmMod.Instance.Logger.Error($"[IL] Unable to match \"{opCode}\" \"{toMatch}\" ({index - 1})!");

        public static void LogPatchCompletion(string methodName) =>
            CataclysmMod.Instance.Logger.Debug($"[IL] Successfully patched {methodName}!");
    }
}