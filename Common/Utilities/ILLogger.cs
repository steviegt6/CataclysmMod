namespace CataclysmMod.Common.Utilities
{
    internal static class ILLogger
    {
        internal static void LogILError(string opCode, string toMatch) => CataclysmMod.Instance.Logger.Error($"[IL] Unable to match \"{opCode}\" \"{toMatch}\"!");

        internal static void LogILError(string opCode, string toMatch, int index) => CataclysmMod.Instance.Logger.Error($"[IL] Unable to match \"{opCode}\" \"{toMatch}\" ({index - 1})!");

        internal static void LogILCompletion(string methodName) => CataclysmMod.Instance.Logger.Debug($"[IL] Successfully patched {methodName}!");
    }
}