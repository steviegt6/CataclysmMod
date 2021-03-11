using System;
using CataclysmMod.Common;
using CataclysmMod.Common.Exceptions;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public static readonly Version ExpectedCalamityVersion = new Version();

        public CataclysmMod()
        {
            Instance = this;

            Properties = new ModProperties
            {
                Autoload = true,
                AutoloadBackgrounds = true,
                AutoloadGores = true,
                AutoloadSounds = true
            };
        }

        public static CataclysmMod Instance { get; private set; }

        public override void Load()
        {
            CalamityVersionException.ThrowErrorOnIncorrectVersion(ModLoader.GetMod("CalamityMod"), ExpectedCalamityVersion);
            ILManager.Load();
        }

        public override void Unload() => ILManager.Unload();

        public override void AddRecipes() => RecipeHandler.AddRecipes();

        public override void PostAddRecipes() => RecipeHandler.ModifyRecipes();

        public override void AddRecipeGroups() => RecipeHandler.AddRecipeGroups();
    }
}