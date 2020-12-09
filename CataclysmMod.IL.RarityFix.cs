using CalamityMod;
using CalamityMod.Items.Accessories;
using CalamityMod.Items.Armor;
using CalamityMod.Items.Tools;
using CalamityMod.Items.Weapons.Magic;
using CalamityMod.Items.Weapons.Melee;
using CalamityMod.Items.Weapons.Ranged;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Items.Weapons.Summon;
using Microsoft.Xna.Framework;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod
{
    public partial class CataclysmMod : Mod
    {
        public static Item[] TextItem = new Item[20];
        public static CalamityRarity[] ItemTextRare = new CalamityRarity[20];

        private void ChangeItemTextColor(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchCall<Color>("get_White")))
            {
                Logger.Warn("[IL] Unable to match call \"Microsoft.Xna.Framework.Color::get_White\"!");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchStfld<ItemText>("color")))
            {
                Logger.Warn("[IL] Unabled to match stfld \"Terraria.ItemText::color\"!");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchStfld<ItemText>("expert")))
            {
                Logger.Warn("[IL] Unabled to match stfld \"Terraria.ItemText::expert\"!");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Ldarg_0); // newItem
            c.Emit(OpCodes.Ldloc_1); // num2 (int32)

            c.EmitDelegate<Action<Item, int>>((newItem, num2) =>
            {
                TextItem[num2] = newItem;
                ItemTextRare[num2] = newItem.Calamity().customRarity;
            });

            Logger.Info("[IL] Finished patching!");
        }

        private void UpdateAnimatedCalamityRarities(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchCallvirt("Terraria.ItemText", "Update")))
            {
                Logger.Warn("[IL] Unable to match callvirt \"Terraria.ItemText::Update\"!");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Ldloc_1); // i (int32)

            c.EmitDelegate<Action<int>>((i) =>
            {
                switch (ItemTextRare[i])
                {
                    case CalamityRarity.DarkBlue:
                        Main.itemText[i].color = new Color(43, 96, 222);
                        break;

                    case CalamityRarity.Dedicated:
                        Main.itemText[i].color = new Color(139, 0, 0);
                        break;

                    case CalamityRarity.Developer:
                        Main.itemText[i].color = new Color(255, 0, 255);
                        break;

                    case CalamityRarity.DraedonRust:
                        Main.itemText[i].color = new Color(204, 71, 35);
                        break;

                    case CalamityRarity.PureGreen:
                        Main.itemText[i].color = new Color(0, 255, 0);
                        break;

                    case CalamityRarity.Rainbow:
                        // Why not use ItemRarityID.Expert?
                        Main.itemText[i].expert = true;
                        break;

                    case CalamityRarity.RareVariant:
                        Main.itemText[i].color = new Color(255, 140, 0);
                        break;

                    case CalamityRarity.Turquoise:
                        Main.itemText[i].color = new Color(0, 255, 200);
                        break;

                    case CalamityRarity.Violet:
                        Main.itemText[i].color = new Color(108, 45, 199);
                        break;

                    case CalamityRarity.ItemSpecific:
                        AssignSpecificColors(TextItem[i], ref Main.itemText[i].color);
                        break;

                    default:
                    case CalamityRarity.NoEffect:
                        break;
                }
            });

            Logger.Info("[IL] Finished patching!");
        }

        private void MouseTextRarityColors(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdsfld<Main>("DiscoB")))
            {
                Logger.Warn("[IL] Unable to match ldsfld \"Terraria.Main.DiscoB\"!");
                return;
            }

            for (int j = 0; j < 3; j++)
                if (!c.TryGotoNext(i => i.MatchCall<Color>(".ctor")))
                {
                    Logger.Warn($"[IL] Unable to match call \"Microsoft.Xna.Framework.Color..ctor\" ({j})!");
                    return;
                }

            c.Index++;

            c.Emit(OpCodes.Ldarg_2); // rare (int)
            c.Emit(OpCodes.Ldloc, 4); // baseColor (Color)

            c.EmitDelegate<Action<int, Color>>((rare, baseColor) =>
            {
                switch (rare)
                {
                    case (int)CalamityRarity.DarkBlue:
                        baseColor = new Color(43, 96, 222);
                        break;

                    case (int)CalamityRarity.Dedicated:
                        baseColor = new Color(139, 0, 0);
                        break;

                    case (int)CalamityRarity.Developer:
                        baseColor = new Color(255, 0, 255);
                        break;

                    case (int)CalamityRarity.DraedonRust:
                        baseColor = new Color(204, 71, 35);
                        break;

                    case (int)CalamityRarity.PureGreen:
                        baseColor = new Color(0, 255, 0);
                        break;

                    case (int)CalamityRarity.RareVariant:
                        baseColor = new Color(255, 140, 0);
                        break;

                    case (int)CalamityRarity.Turquoise:
                        baseColor = new Color(0, 255, 200);
                        break;

                    case (int)CalamityRarity.Violet:
                        baseColor = new Color(108, 45, 199);
                        break;
                }

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Fabstaff>()}"))
                    baseColor = new Color(Main.DiscoR, 100, 255);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<BlushieStaff>()}"))
                    baseColor = new Color(0, 0, 255);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Judgement>()}"))
                    baseColor = Judgement.GetSyncedLightColor();

                if (rare == int.Parse($"{10000}{ModContent.ItemType<NanoblackReaperMelee>()}") || rare == int.Parse($"{rare}{ModContent.ItemType<NanoblackReaperRogue>()}"))
                    baseColor = new Color(0.34f, 0.34f + 0.66f * Main.DiscoG / 255f, 0.34f + 0.5f * Main.DiscoG / 255f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<ProfanedSoulCrystal>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(255, 166, 0), new Color(25, 250, 25), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<BensUmbrella>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(210, 0, 255), new Color(255, 248, 24), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Endogenesis>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(131, 239, 255), new Color(36, 55, 230), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<DraconicDestruction>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(255, 69, 0), new Color(139, 0, 0), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<ScarletDevil>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(191, 45, 71), new Color(185, 187, 253), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<RedSun>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(204, 86, 80), new Color(237, 69, 141), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<GaelsGreatsword>()}"))
                    baseColor = new Color(146, 0, 0);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<CrystylCrusher>()}"))
                    baseColor = new Color(129, 29, 149);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Svantechnical>()}"))
                    baseColor = new Color(220, 20, 60);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<SomaPrime>()}"))
                    baseColor = new Color(254, 253, 235);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Contagion>()}"))
                    baseColor = new Color(207, 17, 117);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>()}") || rare == int.Parse($"{10000}{ModContent.ItemType<TriactisTruePaladinianMageHammerofMight>()}"))
                    baseColor = new Color(227, 226, 180);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<RoyalKnivesMelee>()}") || rare == int.Parse($"{10000}{ModContent.ItemType<RoyalKnives>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(154, 255, 151), new Color(228, 151, 255), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<DemonshadeHelm>()}") || rare == int.Parse($"{10000}{ModContent.ItemType<DemonshadeBreastplate>()}") || rare == int.Parse($"{rare}{ModContent.ItemType<DemonshadeGreaves>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(255, 132, 22), new Color(221, 85, 7), 4f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<PrototypeAndromechaRing>()}"))
                {
                    if (Main.GlobalTime % 1f < 0.6f)
                        baseColor = new Color(89, 229, 255);
                    else if (Main.GlobalTime % 1f < 0.8f)
                        baseColor = Color.Lerp(new Color(89, 229, 255), Color.White, (Main.GlobalTime % 1f - 0.6f) / 0.2f);
                    else
                        baseColor = Color.Lerp(Color.White, new Color(89, 229, 255), (Main.GlobalTime % 1f - 0.8f) / 0.2f);
                }

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Earth>()}"))
                {
                    List<Color> list2 = new List<Color>
                    {
                    new Color(255, 99, 146),
                    new Color(255, 228, 94),
                    new Color(127, 200, 248)
                    };

                    int earthColorIndex = (int)(Main.GlobalTime / 2f % (float)list2.Count);
                    Color earthColor1 = list2[earthColorIndex];
                    Color earthColor2 = list2[(earthColorIndex + 1) % list2.Count];

                    baseColor = Color.Lerp(earthColor1, earthColor2, (Main.GlobalTime % 2f > 1f) ? 1f : (Main.GlobalTime % 1f));
                }

                if (rare == int.Parse($"{10000}{ModContent.ItemType<AegisBlade>()}") || rare == int.Parse($"{10000}{ModContent.ItemType<YharimsCrystal>()}"))
                    baseColor = new Color(255, Main.DiscoG, 53);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<BlossomFlux>()}"))
                    baseColor = new Color(Main.DiscoR, 203, 103);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<BrinyBaron>()}") || rare == int.Parse($"{10000}{ModContent.ItemType<ColdDivinity>()}"))
                    baseColor = new Color(53, Main.DiscoG, 255);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<CosmicDischarge>()}"))
                    baseColor = new Color(150, Main.DiscoG, 255);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Malachite>()}"))
                    baseColor = new Color(Main.DiscoR, 203, 103);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<SeasSearing>()}"))
                    baseColor = new Color(60, Main.DiscoG, 190);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<SHPC>()}"))
                    baseColor = new Color(255, Main.DiscoG, 155);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<Vesuvius>()}"))
                    baseColor = new Color(255, Main.DiscoG, 0);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<PristineFury>()}"))
                    baseColor = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);

                if (rare == int.Parse($"{10000}{ModContent.ItemType<LeonidProgenitor>()}"))
                    baseColor = CalamityUtils.ColorSwap(LeonidProgenitor.blueColor, LeonidProgenitor.purpleColor, 3f);
            });

            Logger.Info("[IL] Finished patching!");
        }

        private void RemovePrefixRarityCap(ILContext il)
        {
            ILCursor c = new ILCursor(il);

            if (!c.TryGotoNext(i => i.MatchLdcI4(-11)))
            {
                Logger.Warn("[IL] Unable to match ldc.i4.s \"-11\"!");
                return;
            }

            if (!c.TryGotoNext(i => i.MatchLdcI4(11)))
            {
                Logger.Warn("[IL] Unable to match ldc.i4.s \"11\"!");
                return;
            }

            c.Index++;

            c.Emit(OpCodes.Pop);
            c.Emit(OpCodes.Ldc_I4, int.MaxValue);

            Logger.Info("[IL] Finished patching!");
        }

        private void AssignSpecificColors(Item item, ref Color color)
        {
            if (item == null)
                return;

            if (item.type == ModContent.ItemType<Fabstaff>())
                color = new Color(Main.DiscoR, 100, 255);

            if (item.type == ModContent.ItemType<BlushieStaff>())
                color = new Color(0, 0, 255);

            if (item.type == ModContent.ItemType<Judgement>())
                color = Judgement.GetSyncedLightColor();

            if (item.type == ModContent.ItemType<NanoblackReaperMelee>() || item.type == ModContent.ItemType<NanoblackReaperRogue>())
                color = new Color(0.34f, 0.34f + 0.66f * Main.DiscoG / 255f, 0.34f + 0.5f * Main.DiscoG / 255f);

            if (item.type == ModContent.ItemType<ProfanedSoulCrystal>())
                color = CalamityUtils.ColorSwap(new Color(255, 166, 0), new Color(25, 250, 25), 4f);

            if (item.type == ModContent.ItemType<BensUmbrella>())
                color = CalamityUtils.ColorSwap(new Color(210, 0, 255), new Color(255, 248, 24), 4f);

            if (item.type == ModContent.ItemType<Endogenesis>())
                color = CalamityUtils.ColorSwap(new Color(131, 239, 255), new Color(36, 55, 230), 4f);

            if (item.type == ModContent.ItemType<DraconicDestruction>())
                color = CalamityUtils.ColorSwap(new Color(255, 69, 0), new Color(139, 0, 0), 4f);

            if (item.type == ModContent.ItemType<ScarletDevil>())
                color = CalamityUtils.ColorSwap(new Color(191, 45, 71), new Color(185, 187, 253), 4f);

            if (item.type == ModContent.ItemType<RedSun>())
                color = CalamityUtils.ColorSwap(new Color(204, 86, 80), new Color(237, 69, 141), 4f);

            if (item.type == ModContent.ItemType<GaelsGreatsword>())
                color = new Color(146, 0, 0);

            if (item.type == ModContent.ItemType<CrystylCrusher>())
                color = new Color(129, 29, 149);

            if (item.type == ModContent.ItemType<Svantechnical>())
                color = new Color(220, 20, 60);

            if (item.type == ModContent.ItemType<SomaPrime>())
                color = new Color(254, 253, 235);

            if (item.type == ModContent.ItemType<Contagion>())
                color = new Color(207, 17, 117);

            if (item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMightMelee>() || item.type == ModContent.ItemType<TriactisTruePaladinianMageHammerofMight>())
                color = new Color(227, 226, 180);

            if (item.type == ModContent.ItemType<RoyalKnivesMelee>() || item.type == ModContent.ItemType<RoyalKnives>())
                color = CalamityUtils.ColorSwap(new Color(154, 255, 151), new Color(228, 151, 255), 4f);

            if (item.type == ModContent.ItemType<DemonshadeHelm>() || item.type == ModContent.ItemType<DemonshadeBreastplate>() || item.type == ModContent.ItemType<DemonshadeGreaves>())
                color = CalamityUtils.ColorSwap(new Color(255, 132, 22), new Color(221, 85, 7), 4f);

            if (item.type == ModContent.ItemType<PrototypeAndromechaRing>())
            {
                if (Main.GlobalTime % 1f < 0.6f)
                    color = new Color(89, 229, 255);
                else if (Main.GlobalTime % 1f < 0.8f)
                    color = Color.Lerp(new Color(89, 229, 255), Color.White, (Main.GlobalTime % 1f - 0.6f) / 0.2f);
                else
                    color = Color.Lerp(Color.White, new Color(89, 229, 255), (Main.GlobalTime % 1f - 0.8f) / 0.2f);
            }

            if (item.type == ModContent.ItemType<Earth>())
            {
                List<Color> list2 = new List<Color>
                {
                    new Color(255, 99, 146),
                    new Color(255, 228, 94),
                    new Color(127, 200, 248)
                };

                int earthColorIndex = (int)(Main.GlobalTime / 2f % (float)list2.Count);
                Color earthColor1 = list2[earthColorIndex];
                Color earthColor2 = list2[(earthColorIndex + 1) % list2.Count];

                color = Color.Lerp(earthColor1, earthColor2, (Main.GlobalTime % 2f > 1f) ? 1f : (Main.GlobalTime % 1f));
            }

            if (item.type == ModContent.ItemType<AegisBlade>() || item.type == ModContent.ItemType<YharimsCrystal>())
                color = new Color(255, Main.DiscoG, 53);

            if (item.type == ModContent.ItemType<BlossomFlux>())
                color = new Color(Main.DiscoR, 203, 103);

            if (item.type == ModContent.ItemType<BrinyBaron>() || item.type == ModContent.ItemType<ColdDivinity>())
                color = new Color(53, Main.DiscoG, 255);

            if (item.type == ModContent.ItemType<CosmicDischarge>())
                color = new Color(150, Main.DiscoG, 255);

            if (item.type == ModContent.ItemType<Malachite>())
                color = new Color(Main.DiscoR, 203, 103);

            if (item.type == ModContent.ItemType<SeasSearing>())
                color = new Color(60, Main.DiscoG, 190);

            if (item.type == ModContent.ItemType<SHPC>())
                color = new Color(255, Main.DiscoG, 155);

            if (item.type == ModContent.ItemType<Vesuvius>())
                color = new Color(255, Main.DiscoG, 0);

            if (item.type == ModContent.ItemType<PristineFury>())
                color = CalamityUtils.ColorSwap(new Color(255, 168, 53), new Color(255, 249, 0), 2f);

            if (item.type == ModContent.ItemType<LeonidProgenitor>())
                color = CalamityUtils.ColorSwap(LeonidProgenitor.blueColor, LeonidProgenitor.purpleColor, 3f);
        }
    }
}
