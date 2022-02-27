#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

namespace CataclysmMod.Core.Loading
{
    /// <summary>
    ///     Called upon mod loading.
    /// </summary>
    public interface ILoadable
    {
        void Load();
    }
}