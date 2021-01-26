using CataclysmMod.Common;
using CataclysmMod.Content.Recipes;
using System;
using System.IO;
using System.Reflection;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public static CataclysmMod Instance { get; private set; }

        public Mod Calamity => ModLoader.GetMod("CalamityMod");

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

            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                if (args.Name.Contains("CalamityMod") && !args.Name.Contains("MMHOOK") && ModLoader.GetMod("CalamityMod") != null)
                    return ModLoader.GetMod("CalamityMod").Code;

                return null;
            };
        }

        public override void Load() => ILManager.Load();

        public override void Unload()
        {
            ILManager.Unload();
            RecipeManager.Unload();
        }

        public override void PostAddRecipes() => RecipeManager.Load();
    }
}