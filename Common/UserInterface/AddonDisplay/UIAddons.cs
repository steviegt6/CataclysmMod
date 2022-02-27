#region License
// Copyright (C) 2021 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ModLoader.UI;
using Terraria.UI;

namespace CataclysmMod.Common.UserInterface.AddonDisplay
{
    public class UIAddons : UIState
    {
        public bool UpdateNeeded = true;
        
        public UIElement BackPanel;
        public UIElement DisplayPanel;
        public UIList AddonsList;
        public UIScrollbar Scrollbar;
        public UITextPanel<string> Header;
        public UIAutoScaleTextTextPanel<string> BackButton;
        public UIElement UpperMenuContainer;
        public UIInputTextField FilterTextBox;

        public readonly List<UIAddonItem> Items = new List<UIAddonItem>();

        public override void OnInitialize()
        {
            base.OnInitialize();

            AddonsList = new UIList();

            foreach (Addon addon in ModContent.GetInstance<Cataclysm>().RegisteredAddons.Values)
            {
                if (addon is AutoloadAddon)
                    continue;
                
                UIAddonItem item = new UIAddonItem(addon);
                item.Activate();
                Items.Add(item);
            }

            BackPanel = new UIElement
            {
                Width = {Percent = 0.8f},
                MaxWidth = UICommon.MaxPanelWidth,
                Top = {Pixels = 220},
                Height = {Pixels = -220, Percent = 1f},
                HAlign = 0.5f
            };

            DisplayPanel = new UIPanel
            {
                Width = {Percent = 1f},
                Height = {Pixels = -110, Percent = 1f},
                BackgroundColor = UICommon.MainPanelBackground,
                PaddingTop = 0f
            };

            BackPanel.Append(DisplayPanel);

            AddonsList = new UIList
            {
                Width = {Pixels = -25, Percent = 1f},
                Height = {Pixels = -50, Percent = 1f},
                Top = {Pixels = 50},
                ListPadding = 5f
            };

            DisplayPanel.Append(AddonsList);

            Scrollbar = new UIScrollbar
            {
                Height = {Pixels = -50, Percent = 1f},
                Top = {Pixels = 50},
                HAlign = 1f
            }.WithView(100f, 1000f);

            DisplayPanel.Append(Scrollbar);

            Header = new UITextPanel<string>(Cataclysm.TextValue("UI.AddonsTitle"), 0.8f, true)
            {
                HAlign = 0.5f,
                Top = {Pixels = -35},
                BackgroundColor = UICommon.DefaultUIBlue
            }.WithPadding(15f);

            BackPanel.Append(Header);

            BackButton = new UIAutoScaleTextTextPanel<string>(Language.GetTextValue("UI.Back"))
            {
                Width = new StyleDimension(-10f, 1f / 3f),
                Height = {Pixels = 40},
                VAlign = 1f,
                HAlign = 0.5f,
                Top = {Pixels = -65}
            };
            BackButton.WithFadedMouseOver();
            BackButton.OnClick += (evt, element) => { Main.menuMode = 0; };
            BackPanel.Append(BackButton);

            UpperMenuContainer = new UIElement
            {
                Width = {Percent = 1f},
                Height = {Pixels = 32},
                Top = {Pixels = 10}
            };

            UIPanel filterTextBoxBackground = new UIPanel
            {
                Top = {Percent = 0f},
                Left = {Pixels = -130, Percent = 1f},
                Width = {Pixels = 135},
                Height = {Pixels = 40}
            };
            filterTextBoxBackground.OnRightClick += (evt, element) => FilterTextBox.Text = "";
            UpperMenuContainer.Append(filterTextBoxBackground);

            FilterTextBox = new UIInputTextField(Cataclysm.TextValue("UI.FilterAddons"))
            {
                Top = {Pixels = 5},
                Left = {Pixels = -120, Percent = 1f},
                Width = {Pixels = 120},
                Height = {Pixels = 20}
            };
            FilterTextBox.OnTextChange += (sender, args) => UpdateNeeded = true;
            UpperMenuContainer.Append(FilterTextBox);

            DisplayPanel.Append(UpperMenuContainer);
            Append(BackPanel);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!UpdateNeeded || FilterTextBox == null)
                return;

            UpdateNeeded = false;
            string filter = FilterTextBox.Text.ToLower();
            
            AddonsList.Clear();
            IEnumerable<UIAddonItem> visible = Items.Where(
                x => x.Addon.DisplayName.ToLower().Contains(filter) || x.Addon.InternalName.ToLower().Contains(filter)
            );

            AddonsList.AddRange(visible);
            Recalculate();
        }
    }
}