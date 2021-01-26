using CataclysmMod.Common.Configs;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Common.Detours
{
    public class SlimeGodDistortionDetour : Detour
    {
        public override string DictKey => "On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.OnHitPlayer";

        public override void Load() => On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.OnHitPlayer += NewOnHitBuff;

        public override void Unload() => On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.OnHitPlayer -= NewOnHitBuff;

        private void NewOnHitBuff(On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.orig_OnHitPlayer orig, CalamityMod.NPCs.SlimeGod.SlimeGodCore self, object player, int damage, bool crit)
        {
            if (player is Player && CalamityChangesConfig.Instance.slimeGodSlimedDebuff)
                (player as Player).AddBuff(BuffID.Slimed, 60 * 2);
            else
                orig(self, player, damage, crit);
        }
    }
}