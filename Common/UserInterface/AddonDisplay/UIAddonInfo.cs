#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;
using Terraria.UI.Gamepad;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
    public class UIAddonInfo : UIState
    {
	    private UIElement BackPanel;
		private UIMessageBox ModInfoDisplay;
		private UITextPanel<string> TitleDisplay;

		private int GotoMenu;
		private Addon Addon;
		private string AddonInfo = string.Empty;
		private string AddonDisplayName = string.Empty;

		public override void OnInitialize() {
			BackPanel = new UIElement {
				Width = {Percent = 0.8f},
				MaxWidth = UICommon.MaxPanelWidth,
				Top = {Pixels = 220},
				Height = {Pixels = -220, Percent = 1f},
				HAlign = 0.5f
			};

			UIPanel uIPanel = new UIPanel
			{
				Width = {Percent = 1f},
				Height = {Pixels = -110, Percent = 1f},
				BackgroundColor = UICommon.MainPanelBackground
			};
			BackPanel.Append(uIPanel);

			ModInfoDisplay = new UIMessageBox(string.Empty) {
				Width = {Pixels = -25, Percent = 1f},
				Height = {Percent = 1f}
			};
			uIPanel.Append(ModInfoDisplay);

			UIScrollbar uIScrollbar = new UIScrollbar {
				Height = {Pixels = -20, Percent = 1f},
				VAlign = 0.5f,
				HAlign = 1f
			}.WithView(100f, 1000f);
			uIPanel.Append(uIScrollbar);

			ModInfoDisplay.SetScrollbar(uIScrollbar);
			TitleDisplay = new UITextPanel<string>(Language.GetTextValue("tModLoader.ModInfoHeader"), 0.8f, true) {
				HAlign = 0.5f,
				Top = {Pixels = -35},
				BackgroundColor = UICommon.DefaultUIBlue
			}.WithPadding(15f);
			BackPanel.Append(TitleDisplay);

			UIAutoScaleTextTextPanel<string> backButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Back")) {
				Width = new StyleDimension(-10f, 1f / 3f),
				Height = {Pixels = 40},
				VAlign = 1f,
				HAlign = 0.5f,
				Top = {Pixels = -65}
			}.WithFadedMouseOver();
			backButton.OnClick += BackClick;
			BackPanel.Append(backButton);

			Append(BackPanel);
		}

		public void Show(Addon addon) {
			Addon = addon;
			AddonDisplayName = addon.DisplayName;
			AddonInfo = addon.Description;

			Main.MenuUI.SetState(ModContent.GetInstance<Cataclysm>().AddonInfoUI);
		}

		public override void OnDeactivate() {
			base.OnDeactivate();

			AddonInfo = string.Empty;
			Addon = null;
			GotoMenu = 0;
			AddonDisplayName = string.Empty;
		}

		private static void BackClick(UIMouseEvent evt, UIElement listeningElement) {
			Main.PlaySound(SoundID.MenuClose);
			Main.MenuUI.SetState(ModContent.GetInstance<Cataclysm>().AddonsUI);
		}

		public override void Draw(SpriteBatch spriteBatch) {
			base.Draw(spriteBatch);

			UILinkPointNavigator.Shortcuts.BackButtonCommand = 100;
			UILinkPointNavigator.Shortcuts.BackButtonGoto = GotoMenu;
		}

		public override void OnActivate() {
			ModInfoDisplay.SetText(AddonInfo);
			TitleDisplay.SetText(AddonDisplayName, 0.8f, true);
		}
    }
}