﻿#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Reflection;
using CataclysmMod.Core;
using CataclysmMod.Core.Loading;
using CataclysmMod.Core.Weaving;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace CataclysmMod.Content.Vanilla.Patches
{
    public class DisplayMenuButtonPatch : ILoadable
    {
        public delegate void AddMenuButtons(Main main,
            int selectedMenu,
            string[] buttonNames,
            float[] buttonScales,
            ref int offY,
            ref int spacing,
            ref int buttonIndex,
            ref int numButtons
        );
        
        public void Load()
        {
            MethodInfo detoured = typeof(ModLoader).Assembly
                .GetCachedType("Terraria.ModLoader.UI.Interface")
                .GetCachedMethod("AddMenuButtons");
            
            HookCreator.Detour(
                detoured,
                GetType().GetCachedMethod(nameof(AddMenuButton))
            );
        }

        public static void AddMenuButton(
            AddMenuButtons orig,
            Main main,
            int selectedMenu,
            string[] buttonNames,
            float[] buttonScales,
            ref int offY,
            ref int spacing,
            ref int buttonIndex,
            ref int numButtons
            )
        {
            buttonNames[buttonIndex] = Cataclysm.TextValue("UI.AddonsButton");

            if (selectedMenu == buttonIndex)
            {
                Main.PlaySound(SoundID.MenuOpen);
                Main.menuMode = 888;
                Main.MenuUI.SetState(ModContent.GetInstance<Cataclysm>().AddonsUI);
            }

            buttonIndex++;
            numButtons++;
            
            orig(main, selectedMenu, buttonNames, buttonScales, ref offY, ref spacing, ref buttonIndex, ref numButtons);
        }
    }
}