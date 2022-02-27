#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Diagnostics;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using Terraria.UI.Chat;

namespace CataclysmMod.Content.Vanilla.ChatTags
{
    public class LinkChatTag : ChatTag
    {
        public class LinkTextSnippet : TextSnippet
        {
            private string Link;
            
            public LinkTextSnippet(string text, Color color, string link) : base(text, color, 1f)
            {
                Link = link;
            }
            
            public override bool UniqueDraw(
                bool justCheckingString,
                out Vector2 size,
                SpriteBatch spriteBatch,
                Vector2 position = new Vector2(),
                Color color = new Color(),
                float scale = 1
            )
            {
                size = Main.fontMouseText.MeasureString(Text);
                
                if (justCheckingString)
                    return false;

                spriteBatch.Draw(
                    Underliner,
                    position + new Vector2(0f, size.Y) * 0.65f,
                    null,
                    color,
                    0f,
                    Vector2.Zero,
                    new Vector2(size.X * scale, scale),
                    SpriteEffects.None,
                    0f
                );

                Rectangle mouseRect = new Rectangle(Main.mouseX, Main.mouseY, 1, 1);
                Rectangle posRect = new Rectangle((int) position.X, (int) position.Y, (int) size.X, (int) size.Y);
                
                if (posRect.Intersects(mouseRect) && Main.mouseLeftRelease && Main.mouseLeft && color != Color.Black)
                    OnClick();
                
                return false;
            }

            public override void OnClick()
            {
                base.OnClick();

                string url;

                switch (Link)
                {
                    case "github":
                        url = "https://github.com/Steviegt6/CataclysmMod";
                        break;
                    
                    case "discord":
                        url = "https://discord.gg/Y8bvvqyFQw";
                        break;
                    
                    default:
                        url = "";
                        break;
                }
                
                Process.Start(url);
            }
        }
        
        private static readonly Texture2D Underliner = new Texture2D(Main.graphics.GraphicsDevice, 1, 2);

        static LinkChatTag()
        {
            Underliner.SetData(new[] {
                Color.White,
                Color.White
            });
        }
        
        protected override IEnumerable<string> Aliases
        {
            get
            {
                yield return "link";
            }
        }

        public override TextSnippet Parse(string text, Color baseColor = new Color(), string options = null) =>
            new LinkTextSnippet(text, Colors.RarityBlue, options ?? "");
    }
}