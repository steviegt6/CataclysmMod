using Terraria.ID;

namespace CataclysmMod.Common.ToolTips
{
    public readonly struct ToolTipReplacementStructure
    {
        public readonly string ToolTipToMatch;
        public readonly string ToolTipReplacement;
        public readonly int ItemId;

        public ToolTipReplacementStructure(string toolTipToMatch, string toolTipReplacement, int itemId = ItemID.None)
        {
            ToolTipToMatch = toolTipToMatch;
            ToolTipReplacement = toolTipReplacement;
            ItemId = itemId;
        }
    }
}