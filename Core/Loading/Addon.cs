﻿#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System;
using Terraria.ModLoader;

namespace CataclysmMod.Core.Loading
{
    /// <summary>
    ///     Represents an informational object for mod load handling.
    /// </summary>
    public abstract class Addon
    {
        /// <summary>
        ///     The internal name of the mod.
        /// </summary>
        public abstract string InternalName { get; }

        /// <summary>
        ///     The minimum expected version of the mod.
        /// </summary>
        public abstract Version MinimumVersion { get; }
        
        /// <summary>
        ///     Whether this mod is enabled.
        /// </summary>
        public virtual bool IsEnabled => ModLoader.GetMod(InternalName) != null;
    }
    
    /// <inheritdoc cref="Addon"/>
    /// <typeparam name="TSelf">Self-referential generic type.</typeparam>
    // Self-referencing generic for CLR abuse.
    public abstract class Addon<TSelf> : Addon where TSelf : Addon<TSelf>, new()
    {
        private static TSelf Self;

        /// <summary>
        ///     A singleton addon instance.
        /// </summary>
        public static TSelf Instance => Self ?? (Self = new TSelf());
    }
}