using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace CataclysmMod.Common.Utilities
{
    public static class TooltipUtils
    {
        public static readonly List<string> NamesBeforeTooltip = new List<string>
        {
            "Material",
            "Consumable",
            "Ammo",
            "Placeable",
            "UseMana",
            "HealMana",
            "HealLife",
            "TileBoost",
            "HammerPower",
            "AxePower",
            "PickPower",
            "Defense",
            "Vanity",
            "Quest",
            "WandConsumes",
            "Equipable",
            "BaitPower",
            "NeedsBait",
            "FishingPower",
            "Knockback",
            "Speed",
            "CritChance",
            "Damage",
            "SocialDesc",
            "Social",
            "FavoriteDesc",
            "Favorite",
            "ItemName"
        };

        public static void AddTooltip(List<TooltipLine> lines, TooltipLine line)
        {
            int index;

            if (lines.FirstOrDefault(x => x.Name.Equals("Tooltip0") && x.mod.Equals("Terraria")) == null)
            {
                foreach (string tooltip in NamesBeforeTooltip)
                {
                    if (lines.FirstOrDefault(x => x.Name.Equals(tooltip) && x.mod.Equals("Terraria")) != null)
                    {
                        index = lines.IndexOf(lines.First(x => x.Name.Equals(tooltip) && x.mod.Equals("Terraria")));
                        lines.Insert(index, line);
                        return;
                    }
                }

                return;
            }

            int i = 0;
            while (true)
            {
                if (lines.FirstOrDefault(x => x.Name.Equals($"Tooltip{i}") && x.mod.Equals("Terraria")) != null)
                    i++;
                else
                {
                    i--;
                    break;
                }
            }

            index = lines.IndexOf(lines.First(x => x.Name.Equals($"Tooltip{i}") && x.mod.Equals("Terraria")));
            lines.Insert(index, line);
        }
    }
}