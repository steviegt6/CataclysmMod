#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
	public class UIMessageBox : UIPanel
	{
		protected UIScrollbar Scrollbar;

		private string Text;
		private float TheHeight;
		private bool HeightNeedsRecalculating;
		private readonly List<Tuple<string, float>> DrawTexts = new List<Tuple<string, float>>();

		public UIMessageBox(string text)
		{
			SetText(text);
		}

		public override void OnActivate()
		{
			base.OnActivate();
			HeightNeedsRecalculating = true;
		}

		internal void SetText(string text)
		{
			Text = text;
			ResetScrollbar();
		}

		private void ResetScrollbar()
		{
			if (Scrollbar == null)
				return;
			
			Scrollbar.ViewPosition = 0;
			HeightNeedsRecalculating = true;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			CalculatedStyle space = GetInnerDimensions();
			DynamicSpriteFont font = Main.fontMouseText;
			float position = 0f;
			if (Scrollbar != null)
			{
				position = -Scrollbar.GetValue();
			}

			Tuple<string, float>[] drawTexts = DrawTexts.ToArray();
			foreach (Tuple<string, float> drawText in drawTexts.TakeWhile(drawText => !(position + drawText.Item2 > space.Height)))
			{
				if (position >= 0)
					Utils.DrawBorderString(spriteBatch, drawText.Item1, new Vector2(space.X, space.Y + position),
						Color.White);
				position += drawText.Item2;
			}

			Recalculate();
		}

		public override void RecalculateChildren()
		{
			base.RecalculateChildren();
			
			if (!HeightNeedsRecalculating)
			{
				return;
			}

			CalculatedStyle space = GetInnerDimensions();
			if (space.Width <= 0 || space.Height <= 0)
			{
				return;
			}

			DynamicSpriteFont font = Main.fontMouseText;
			DrawTexts.Clear();
			float position = 0f;
			float textHeight = font.MeasureString("A").Y;
			
			foreach (string line in Text.Split('\n'))
			{
				string drawString = line;
				do
				{
					string remainder = "";
					while (font.MeasureString(drawString).X > space.Width)
					{
						remainder = drawString[drawString.Length - 1] + remainder;
						drawString = drawString.Substring(0, drawString.Length - 1);
					}

					if (remainder.Length > 0)
					{
						int index = drawString.LastIndexOf(' ');
						if (index >= 0)
						{
							remainder = drawString.Substring(index + 1) + remainder;
							drawString = drawString.Substring(0, index);
						}
					}

					DrawTexts.Add(new Tuple<string, float>(drawString, textHeight));
					position += textHeight;
					drawString = remainder;
				} while (drawString.Length > 0);
			}

			TheHeight = position;
			HeightNeedsRecalculating = false;
		}

		public override void Recalculate()
		{
			base.Recalculate();
			
			UpdateScrollbar();
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);
			
			if (Scrollbar != null) Scrollbar.ViewPosition -= evt.ScrollWheelValue;
		}

		public void SetScrollbar(UIScrollbar scrollbar)
		{
			Scrollbar = scrollbar;
			UpdateScrollbar();
			HeightNeedsRecalculating = true;
		}

		private void UpdateScrollbar()
		{
			Scrollbar?.SetView(GetInnerDimensions().Height, TheHeight);
		}
	}
}