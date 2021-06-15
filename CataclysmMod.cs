using System;
using CataclysmMod.Common;
using CataclysmMod.Common.Exceptions;
using CataclysmMod.Content.Configs;
using CataclysmMod.Content.GlobalModifications.Items;
using CataclysmMod.Content.GlobalModifications.Projectiles;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public class CataclysmMod : Mod
    {
        public static Version ExpectedCalamityVersion => new Version(1, 4, 5, 7);

        public static CataclysmMod Instance { get; private set; }

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

        public override void Load()
        {
            CalamityVersionException.ThrowErrorOnIncorrectVersion(ExpectedCalamityVersion);

            ApplicableManager.Load();
            SummonRotationAdjustmentsGlobalProj.Initialize();
            AbyssalMinesExplosionGlobalProj.Initialize();
            ArmorSetDatabase.Initialize();
            TooltipModificationsItem.Initialize();
        }

        public override void Unload()
        {
            ApplicableManager.Unload();
            CataclysmConfig.Instance = null;
            Instance = null;
        }

        public override void AddRecipes() => RecipeHandler.AddRecipes();

        public override void PostAddRecipes() => RecipeHandler.ModifyRecipes();

        public override void AddRecipeGroups() => RecipeHandler.AddRecipeGroups();
    }
}