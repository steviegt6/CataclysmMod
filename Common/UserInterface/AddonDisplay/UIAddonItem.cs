#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Rejuvena.Backscatter.Cache;
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
		private const float Padding = 5f;

		public Addon Addon;
		
		public UIImage MoreInfoButton;
		public UIImage AddonIcon;
		public UIImage ConfigButton;
		public UIText AddonDisplayName;
		public string Tooltip;
		public UIAddonStateText AddonStateText;

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

			string text = $"{Addon.DisplayName} {Cataclysm.TextValue("UI.VersionAbove", Addon.MinimumVersion)}";

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

			AddonStateText = new UIAddonStateText(Addon.IsEnabled)
			{
				Top = {Pixels = 40},
				Left = {Pixels = 85f}
			};
			Append(AddonStateText);

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
				Left = {Pixels = MoreInfoButton.Left.Pixels - 36 - Padding, Precent = 1f},
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
				Tooltip = Cataclysm.TextValue("UI.AddonChanges");
			else if (ConfigButton?.IsMouseHovering == true)
				Tooltip = Cataclysm.TextValue("UI.OpenConfig");
			else if (AddonStateText.IsMouseHovering)
				Tooltip = Addon.IsEnabled
					? Cataclysm.TextValue("UI.IsEnabled", Addon.DisplayName)
					: Cataclysm.TextValue("UI.IsDisabled", Addon.DisplayName);
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
			ModContent.GetInstance<Cataclysm>().AddonInfoUI.Show(Addon);
		}

		internal void OpenConfig(UIMouseEvent evt, UIElement listeningElement)
		{
			if (Addon.Config == null)
				return;
			
			Main.PlaySound(SoundID.MenuOpen);
			ModConfigSetMod(Addon.Config);
			Main.menuMode = 10024; // Interface.modConfigID;
		}

		internal static void ModConfigSetMod(ModConfig config)
		{
			Mod mod = ModContent.GetInstance<Cataclysm>();
			
			Assembly tml = typeof(ModLoader).Assembly;
			Type modConfigUI = tml.GetCachedTypeNotNull("Terraria.ModLoader.Config.UI.UIModConfig");
			Type interfaceT = tml.GetCachedTypeNotNull("Terraria.ModLoader.UI.Interface");
			Type configManager = tml.GetCachedTypeNotNull("Terraria.ModLoader.Config.ConfigManager");
			// ReSharper disable once PossibleNullReferenceException
			object modConfigUIInstance = interfaceT.GetCachedFieldNotNull("modConfig").GetValue(null);
			FieldInfo modField = modConfigUI.GetCachedFieldNotNull("mod");
			FieldInfo modConfigsField = modConfigUI.GetCachedFieldNotNull("modConfigs");
			FieldInfo modConfigField = modConfigUI.GetCachedFieldNotNull("modConfig");
			FieldInfo configsField = configManager.GetCachedFieldNotNull("Configs");
			
			modField.SetValue(modConfigUIInstance, mod);
			modConfigsField.SetValue(modConfigUIInstance, ReflectionCache.GetValue<IDictionary<Mod, List<ModConfig>>>(configsField)[mod]);
			modConfigField.SetValue(modConfigUIInstance, config);
		}
	}
}