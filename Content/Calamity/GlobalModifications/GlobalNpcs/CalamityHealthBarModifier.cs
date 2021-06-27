using CalamityMod;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Content.Default.GlobalModifications;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI.Chat;

namespace CataclysmMod.Content.Calamity.GlobalModifications.GlobalNpcs
{
    [ModDependency("CalamityMod")]
    public class CalamityHealthBarModifier : CataclysmGlobalNpc
    {
        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            string organicDrawText = "";

            if (npc.Organic())
                organicDrawText = "Organic";
            else if (npc.Inorganic())
                organicDrawText = "Inorganic";

            // TODO: re-add config
            // if (string.IsNullOrEmpty(organicDrawText) || !CataclysmConfig.Instance.DisplayOrganicTextNpCs)
            //    return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);

            Color drawColor = Lighting.GetColor((int) (npc.position.X / 16), (int) (npc.position.Y / 16));
            Vector2 drawPos = position - Main.screenPosition;
            Vector2 extraOffsetFromText = new Vector2(Main.fontMouseText.MeasureString(organicDrawText).X / 2f,
                -(Main.fontMouseText.MeasureString(organicDrawText).Y / 2f));
            drawPos -= extraOffsetFromText;
            ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Main.fontMouseText, organicDrawText,
                drawPos, drawColor, 0f, Vector2.Zero, Vector2.One);

            return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
        }
    }
}