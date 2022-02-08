#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
    public class UIAddonStateText : UIElement
    {
        private bool Enabled;

        private string DisplayText => Enabled
            ? Language.GetTextValue("GameUI.Enabled")
            : Language.GetTextValue("GameUI.Disabled");

        private Color DisplayColor => Enabled ? Color.Green : Color.Red;
        
        public UIAddonStateText(bool enabled = true) {
            Enabled = enabled;
            PaddingLeft = PaddingRight = 5f;
            PaddingBottom = PaddingTop = 10f;
        }
        
        public override void Recalculate() {
            Vector2 textSize = new Vector2(Main.fontMouseText.MeasureString(DisplayText).X, 16f);
            Width.Set(textSize.X + PaddingLeft + PaddingRight, 0f);
            Height.Set(textSize.Y + PaddingTop + PaddingBottom, 0f);
            base.Recalculate();
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch) {
            base.DrawSelf(spriteBatch);
            DrawPanel(spriteBatch);
            DrawEnabledText(spriteBatch);
        }

        private void DrawPanel(SpriteBatch spriteBatch) {
            Vector2 position = GetDimensions().Position();
            float width = Width.Pixels;
            spriteBatch.Draw(UICommon.InnerPanelTexture, position, new Rectangle(0, 0, 8, UICommon.InnerPanelTexture.Height), Color.White);
            spriteBatch.Draw(UICommon.InnerPanelTexture, new Vector2(position.X + 8f, position.Y), new Rectangle(8, 0, 8, UICommon.InnerPanelTexture.Height), Color.White, 0f, Vector2.Zero, new Vector2((width - 16f) / 8f, 1f), SpriteEffects.None, 0f);
            spriteBatch.Draw(UICommon.InnerPanelTexture, new Vector2(position.X + width - 8f, position.Y), new Rectangle(16, 0, 8, UICommon.InnerPanelTexture.Height), Color.White);
        }

        private void DrawEnabledText(SpriteBatch spriteBatch) {
            Vector2 pos = GetDimensions().Position() + new Vector2(PaddingLeft, PaddingTop * 0.5f);
            Utils.DrawBorderString(spriteBatch, DisplayText, pos, DisplayColor);
        }
    }
}