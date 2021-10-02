using Terraria.ID;

namespace CataclysmMod.Common.DataStructures
{
    public readonly struct ToolTipReplacement
    {
        public readonly string Match;
        public readonly string Replacement;
        public readonly int ItemId;

        public ToolTipReplacement(string match, string replacement, int itemId = ItemID.None)
        {
            Match = match;
            Replacement = replacement;
            ItemId = itemId;
        }
    }
}