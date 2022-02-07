#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
	public class UIAddonItem : UIPanel
	{
		private const float PADDING = 5f;

		public Addon Addon;
		
		public UIImage MoreInfoButton;
		public UIImage AddonIcon;
		public UIImage ConfigButton;
		public UIText AddonDisplayName;
		public string Tooltip;

		public UIAddonItem(Addon addon)
		{
			Addon = addon;
			
			BorderColor = new Color(89, 116, 213) * 0.7f;
			Height.Pixels = 90;
			Width.Percent = 1f;

			SetPadding(6f);
		}

		public override void OnInitialize()
		{
			base.OnInitialize();

			string text = Addon.DisplayName + " v" + Addon.MinimumVersion + " or above";

			Texture2D iconTexture = ModContent.GetTexture(Addon.Texture);

			AddonIcon = new UIImage(iconTexture)
			{
				Left = {Percent = 0f},
				Top = {Percent = 0f}
			};
			Append(AddonIcon);

			AddonDisplayName = new UIText(text)
			{
				Left = new StyleDimension(85f, 0f),
				Top = {Pixels = 5}
			};
			Append(AddonDisplayName);

			MoreInfoButton = new UIImage(UICommon.ButtonModInfoTexture)
			{
				Width = {Pixels = 36},
				Height = {Pixels = 36},
				Left = {Pixels = -36, Precent = 1},
				Top = {Pixels = 40}
			};
			MoreInfoButton.OnClick += ShowMoreInfo;
			Append(MoreInfoButton);

			if (Addon.Config == null)
				return;
			
			ConfigButton = new UIImage(UICommon.ButtonModConfigTexture)
			{
				Width = {Pixels = 36},
				Height = {Pixels = 36f},
				Left = {Pixels = MoreInfoButton.Left.Pixels - 36 - PADDING, Precent = 1f},
				Top = {Pixels = 40f}
			};
			ConfigButton.OnClick += OpenConfig;
			Append(ConfigButton);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			Tooltip = null;
			
			base.Draw(spriteBatch);

			if (string.IsNullOrEmpty(Tooltip)) 
				return;
			
			Rectangle bounds = GetOuterDimensions().ToRectangle();
			bounds.Height += 16;
				
			UICommon.DrawHoverStringInBounds(spriteBatch, Tooltip, bounds);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			base.DrawSelf(spriteBatch);
			
			CalculatedStyle innerDimensions = GetInnerDimensions();
			Vector2 drawPos = new Vector2(innerDimensions.X + 90f, innerDimensions.Y + 30f);
			spriteBatch.Draw(UICommon.DividerTexture, drawPos, null, Color.White, 0f, Vector2.Zero, new Vector2((innerDimensions.Width - 95f) / 8f, 1f), SpriteEffects.None, 0f);

			if (MoreInfoButton?.IsMouseHovering == true)
			{
				Tooltip = "View addon changes";
			}
			else if (ConfigButton?.IsMouseHovering == true)
			{
				Tooltip = "Open addon config";
			}
		}

		public override void MouseOver(UIMouseEvent evt)
		{
			base.MouseOver(evt);
			BackgroundColor = UICommon.DefaultUIBlue;
			BorderColor = new Color(89, 116, 213);
		}

		public override void MouseOut(UIMouseEvent evt)
		{
			base.MouseOut(evt);
			BackgroundColor = new Color(63, 82, 151) * 0.7f;
			BorderColor = new Color(89, 116, 213) * 0.7f;
		}

		internal void ShowMoreInfo(UIMouseEvent evt, UIElement listeningElement)
		{
			Main.PlaySound(SoundID.MenuOpen);
			
			// TODO: This
			//Interface.modInfo.Show(ModName, _mod.DisplayName, Interface.modsMenuID, _mod, _mod.properties.description,
			//	_mod.properties.homepage);
		}

		internal void OpenConfig(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Addon.Config == null)
				return;
			
			Main.PlaySound(SoundID.MenuOpen);
			Assembly tml = typeof(ModLoader).Assembly;
			Type modConfigUI = tml.GetType("Terraria.ModLoader.UI.UIModConfig");
			Type interfaceT = tml.GetType("Terraria.ModLoader.UI.Interface");
			// ReSharper disable once PossibleNullReferenceException
			object modConfigUIInstance = interfaceT.GetField(
				"modConfig",
				BindingFlags.Static | BindingFlags.NonPublic
			).GetValue(null);
			FieldInfo modField = modConfigUI.GetField("mod", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo modConfigsField = modConfigUI.GetField("modConfigs", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo modConfigField = modConfigUI.GetField("modConfig", BindingFlags.Instance | BindingFlags.NonPublic);

			if (modField == null || modConfigsField == null || modConfigField == null)
				return;
			
			modField.SetValue(modConfigUIInstance, ModContent.GetInstance<Cataclysm>());
			modConfigField.SetValue(modConfigUIInstance, new List<ModConfig> { Addon.Config });
			modConfigField.SetValue(modConfigUIInstance, Addon.Config);
			
			Main.menuMode = 10024; // Interface.modConfigID;
		}
	}
}