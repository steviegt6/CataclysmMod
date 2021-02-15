using CataclysmMod.Content.Configs;
using On.CalamityMod.NPCs.SlimeGod;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Common.MonoMod.ILEdits
{
    public class SlimeGodDistortionDetour : ILEdit
    {
        public override string DictKey => "On.CalamityMod.NPCs.SlimeGod.SlimeGodCore.OnHitPlayer";

        // Don't check for slimeGodSlimedDebuff since we can just call the orig method without the need to reload

        public override void Load() => SlimeGodCore.OnHitPlayer += NewOnHitBuff;

        public override void Unload() => SlimeGodCore.OnHitPlayer -= NewOnHitBuff;

        private static void NewOnHitBuff(SlimeGodCore.orig_OnHitPlayer orig, CalamityMod.NPCs.SlimeGod.SlimeGodCore self, object player, int damage, bool crit)
        {
            if (player is Player vanillaPlayer && CataclysmConfig.Instance.slimeGodSlimedDebuff)
                vanillaPlayer.AddBuff(BuffID.Slimed, 60 * 2);
            else
                orig(self, player, damage, crit);
        }
    }
}