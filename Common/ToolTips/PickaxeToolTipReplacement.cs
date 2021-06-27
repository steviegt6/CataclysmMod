using Terraria.ID;

namespace CataclysmMod.Common.ToolTips
{
    public readonly struct PickaxeToolTipReplacement
    {
        public readonly string ToolTipToMatch;
        public readonly string ToolTipReplacement;
        public readonly int ItemId;

        public PickaxeToolTipReplacement(string toolTipToMatch, string toolTipReplacement, int itemId = ItemID.None)
        {
            ToolTipToMatch = toolTipToMatch;
            ToolTipReplacement = toolTipReplacement;
            ItemId = itemId;
        }
    }
}