using CataclysmMod.Common.Configs;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using ReLogic.OS;
using Terraria.ModLoader;

namespace CataclysmMod.Common.ILEdits
{
    public class FungalClumpDamageIL : ILEdit
    {
        public override string DictKey => "CalamityMod.Items.Accessories.FungalClump.UpdateAccessory";

        public override bool Autoload() => CalamityChangesConfig.Instance.fungalClumpTrueDamage;

        public override void Load() => IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory += RemoveSummonDamageBonus;

        public override void Unload() => IL.CalamityMod.Items.Accessories.FungalClump.UpdateAccessory -= RemoveSummonDamageBonus;

        private void RemoveSummonDamageBonus(ILContext il)
        {
            // FNA IL for this method seems to be different.
            if (Platform.IsWindows)
            {
                ILCursor c = new ILCursor(il);

                /* Match the base float damage of Fungal Clumps
                 * // Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, -1f, ModContent.ProjectileType<FungalClumpMinion>(), (int)(10f * player.MinionDamage()), 1f, player.whoAmI);
                 * IL_0047: ldarg.1
                 * IL_0048: callvirt instance valuetype [Microsoft.Xna.Framework]Microsoft.Xna.Framework.Vector2 [Terraria]Terraria.Entity::get_Center()
                 * IL_004d: ldfld float32 [Microsoft.Xna.Framework]Microsoft.Xna.Framework.Vector2::X
                 * IL_0052: ldarg.1
                 * IL_0053: callvirt instance valuetype [Microsoft.Xna.Framework]Microsoft.Xna.Framework.Vector2 [Terraria]Terraria.Entity::get_Center()
                 * IL_0058: ldfld float32 [Microsoft.Xna.Framework]Microsoft.Xna.Framework.Vector2::Y
                 * IL_005d: ldc.r4 0.0
                 * IL_0062: ldc.r4 -1
                 * IL_0067: call int32 [Terraria]Terraria.ModLoader.ModContent::ProjectileType<class CalamityMod.Projectiles.Summon.FungalClumpMinion>()
                 * IL_006c: ldc.r4 10
                 * IL_0071: ldarg.1
                 * IL_0072: call float32 CalamityMod.CalamityUtils::MinionDamage(class [Terraria]Terraria.Player)
                 * IL_0077: mul
                 * IL_0078: conv.i4
                 * IL_0079: ldc.r4 1
                 * IL_007e: ldarg.1
                 * IL_007f: ldfld int32 [Terraria]Terraria.Entity::whoAmI
                 * IL_0084: ldc.r4 0.0
                 * IL_0089: ldc.r4 0.0
                 * // {
                 * IL_008e: call int32 [Terraria]Terraria.Projectile::NewProjectile(float32, float32, float32, float32, int32, int32, float32, int32, float32, float32)
                 * // }
                 * IL_0093: pop
                 */
                if (!c.TryGotoNext(i => i.MatchLdcR4(10)))
                {
                    ModContent.GetInstance<CataclysmMod>().Logger.Warn("[IL] Unable to match ldc.r4 \"10\"!");
                    return;
                }

                c.Index++;

                // Replace with a regular int
                c.Emit(OpCodes.Pop);
                c.Emit(OpCodes.Ldc_I4, 10);

                c.Index++;

                /* Remove the IL code responsible for multiplying it based on summon damage
                 * IL_0071: ldarg.1
                 * IL_0072: call float32 CalamityMod.CalamityUtils::MinionDamage(class [Terraria]Terraria.Player)
                 * IL_0077: mul
                 * IL_0078: conv.i4
                 */
                c.RemoveRange(4);

                ModContent.GetInstance<CataclysmMod>().Logger.Info("[IL] Finished patching!");
            }
            else
                ModContent.GetInstance<CataclysmMod>().Logger.Warn("[IL] Linux or Mac OS detected, skipping Funal Clump IL editing...");
        }
    }
}