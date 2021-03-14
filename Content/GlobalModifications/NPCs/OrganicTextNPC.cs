using CalamityMod;
using CataclysmMod.Content.Configs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI.Chat;

namespace CataclysmMod.Content.GlobalModifications.NPCs
{
    public class OrganicTextNPC : GlobalNPC
    {
        public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
        {
            string organicDrawText = "";

            if (npc.Organic())
                organicDrawText = "Organic";
            else if (npc.Inorganic())
                organicDrawText = "Inorganic";

            if (string.IsNullOrEmpty(organicDrawText) || !CataclysmConfig.Instance.displayOrganicTextNPCs)
                return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);

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
