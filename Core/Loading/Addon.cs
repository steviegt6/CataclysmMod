#region License

// Copyright (C) 2021 Tomat and Contributors, MIT License

#endregion

using System;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace CataclysmMod.Core.Loading
{
    /// <summary>
    ///     Represents an informational object for mod load handling.
    /// </summary>
    public abstract class Addon
    {
        /// <summary>
        ///     The location of the addon's texture icon.
        /// </summary>
        public virtual string Texture => (GetType().Namespace + "." + InternalName).Replace('.', '/');

        /// <summary>
        ///     The internal name of the mod.
        /// </summary>
        public abstract string InternalName { get; }

        /// <summary>
        ///     The display name of the mod.
        /// </summary>
        public abstract string DisplayName { get; }

        /// <summary>
        ///     The minimum expected version of the mod.
        /// </summary>
        public abstract Version MinimumVersion { get; }

        /// <summary>
        ///     Whether this mod is enabled.
        /// </summary>
        public virtual bool IsEnabled => ModLoader.GetMod(InternalName) != null;

        /// <summary>
        ///     The addon's configuration menu.
        /// </summary>
        public virtual ModConfig Config => null;

        /// <summary>
        ///     Vanity display for changes.
        /// </summary>
        public virtual string Description => "";
    }

    /// <inheritdoc cref="Addon"/>
    /// <typeparam name="TSelf">Self-referential generic type.</typeparam>
    // Self-referencing generic for CLR abuse.
    public abstract class Addon<TSelf> : Addon where TSelf : Addon<TSelf>, new()
    {
        private static TSelf Self;

        /// <summary>
        ///     A singleton addon instance. Produces an instance of itself if none was previously initialized.
        /// </summary>
        public static TSelf Instance => Self ?? (Self = new TSelf());
    }
}