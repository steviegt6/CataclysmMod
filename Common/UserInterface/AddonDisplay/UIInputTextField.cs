#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
    public class UIInputTextField : UIElement
    {
        private readonly string HintText;
        public string _currentString = string.Empty;
        public int _textBlinkerCount;

        public string Text
        {
            get => _currentString;
            
            set
            {
                if (_currentString == value)
                    return;
                
                _currentString = value;
                OnTextChange?.Invoke(this, EventArgs.Empty);
            }
        }

        public delegate void EventHandler(object sender, EventArgs e);

        public event EventHandler OnTextChange;

        public UIInputTextField(string hintText)
        {
            HintText = hintText;
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            PlayerInput.WritingText = true;
            Main.instance.HandleIME();
            string newString = Main.GetInputText(_currentString);

            if (newString != _currentString)
            {
                _currentString = newString;
                OnTextChange?.Invoke(this, EventArgs.Empty);
            }

            string displayString = _currentString;

            if (++_textBlinkerCount / 20 % 2 == 0)
                displayString += "|";

            CalculatedStyle space = GetDimensions();

            if (_currentString.Length == 0)
                Utils.DrawBorderString(spriteBatch, HintText, new Vector2(space.X, space.Y), Color.Gray);
            else
                Utils.DrawBorderString(spriteBatch, displayString, new Vector2(space.X, space.Y), Color.White);
        }
    }
}