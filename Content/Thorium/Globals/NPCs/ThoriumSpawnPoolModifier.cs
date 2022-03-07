#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using CataclysmMod.Common.Addons;
using CataclysmMod.Core.Loading;
using Terraria.ModLoader;
using ThoriumMod.NPCs.Depths;

namespace CataclysmMod.Content.Thorium.Globals.NPCs
{
    [AddonContent(typeof(ThoriumModAddon))]
    public class ThoriumSpawnPoolModifier : GlobalNPC
    {
        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            if (pool.ContainsKey(ModContent.NPCType<Globee>())) 
                pool[ModContent.NPCType<Globee>()] *= 2f;
        }
    }
}