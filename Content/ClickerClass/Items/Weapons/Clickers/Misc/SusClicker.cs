#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Core.Localization;
using ClickerClass;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.ClickerClass.Items.Weapons.Clickers.Misc
{
    public class SusClicker : BaseClickerItem
    {
        public static readonly List<string> AvailableEffects = new List<string>();
        public static int EffectIndex;

        public override void SetStaticDefaults()
        {
            base.SetStaticDefaults();

            ClickerCompatibilityCalls.RegisterClickEffect(
                mod,
                "Impostor",
                FilelessEntries.GetClickEffect("Impostor"),
                FilelessEntries.GetClickDescription("Impostor"),
                3,
                Color.Red,
                PerformSuspiciousActivities
            );

            foreach (string effect in ClickerCompatibilityCalls.GetAllEffectNames().Where(
                         effect => effect != $"{mod.Name}:Impostor")
                    ) AvailableEffects.Add(effect);

            void PerformSuspiciousActivities(Player player, Vector2 position, int type, int damage, float knockBack)
            {
                if (ClickerSystem.IsClickEffect(AvailableEffects[EffectIndex], out ClickEffect effect))
                    effect.Action?.Invoke(player, position, type, damage, knockBack);
            }
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            ClickerCompatibilityCalls.SetRadius(item, 15f);
            ClickerCompatibilityCalls.SetColor(item, Color.Red);
            ClickerCompatibilityCalls.SetDust(item, DustID.Buggy);
            ClickerCompatibilityCalls.AddEffect(item, $"{mod.Name}:Impostor");

            item.damage = 150;
            item.knockBack = 10f;
            item.value = 0;
            item.rare = ItemRarityID.Purple;
        }

        public override bool AltFunctionUse(Player player)
        {
            if (!(Main.mouseRight && Main.mouseRightRelease))
                return false;

            EffectIndex++;

            if (EffectIndex >= AvailableEffects.Count)
                EffectIndex = 0;

            if (ClickerSystem.IsClickEffect(AvailableEffects[EffectIndex], out ClickEffect effect))
                CombatText.NewText(
                    player.getRect(),
                    new Color(Main.rand.Next(0, 256),
                        Main.rand.Next(0, 256),
                        Main.rand.Next(0, 256)),
                    $"Selected: {effect.DisplayName}"
                );

            return true;
        }
    }
}