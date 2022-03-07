#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI.Chat;

namespace CataclysmMod.Core.Loading
{
    public abstract class ChatTag : ITagHandler, ILoadable
    {
        protected abstract IEnumerable<string> Aliases { get; }

        public virtual void Load()
        {
            IEnumerable<string> aliases = Aliases;

            ConcurrentDictionary<string, ITagHandler> handlers =
                (ConcurrentDictionary<string, ITagHandler>)
                typeof(ChatManager)
                    .GetCachedField("_handlers")
                    .GetValue(null);

            foreach (string alias in aliases) 
                handlers[alias.ToLower()] = this;
        }

        public abstract TextSnippet Parse(string text, Color baseColor = new Color(), string options = null);
    }
}