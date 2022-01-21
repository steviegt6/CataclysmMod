#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

namespace CataclysmMod.Core.Loading
{
    /// <summary>
    ///     Represents an informational object for mod load handling.
    /// </summary>
    // Self-referencing generic for CLR abuse.
    public abstract class Addon<TSelf> where TSelf : Addon<TSelf>, new()
    {
        private static TSelf Self;

        public static TSelf Instance => Self ?? (Self = new TSelf());

        /// <summary>
        ///     The internal name of the mod.
        /// </summary>
        public abstract string InternalName { get; }
    }
}