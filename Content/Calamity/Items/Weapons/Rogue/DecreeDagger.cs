using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Projectiles.Rogue;
using CataclysmMod.Content.Calamity.Projectiles;
using CataclysmMod.Core.ModCompatibility;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Calamity.Items.Weapons.Rogue
{
    [ModDependency("CalamityMod")]
    public class DecreeDagger : RogueCataclysmWeapon
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CursedDagger";

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dagger of Decree");
            Tooltip.SetDefault("Throws two daggers, with the top dagger having homing and splitting capabilities" +
                               "\nStealth strikes cause the top dagger to split into two more on death," +
                               "\ngives the bottom dagger higher pierce, " +
                               "\nand both daggers become showered in cursed fireballs");
        }

        public override void SetDefaults()
        {
            base.SetDefaults();

            item.width = 24;
            item.height = 34;
            item.damage = 26;
            item.noMelee = item.noUseGraphic = true;
            item.useAnimation = item.useTime = 18;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.knockBack = 5f;
            item.UseSound = SoundID.Item1;
            item.shoot = ModContent.ProjectileType<CursedDaggerProj>();
            item.shootSpeed = 12f;
            item.GetGlobalItem<CalamityGlobalItem>().rogue = true;
            item.autoReuse = true;
            item.value = Item.buyPrice(gold: 36);
            item.rare = ItemRarityID.Pink;
            item.Calamity().customRarity = CalamityRarity.RareVariant;
        }

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY,
            ref int type, ref int damage, ref float knockBack)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                Vector2 velocity = new Vector2(speedX, speedY);
                velocity *= player.Calamity().StealthStrikeAvailable() ? 1.25f : 1f;

                Projectile normDagger =
                    Projectile.NewProjectileDirect(position, velocity, type, damage, knockBack, player.whoAmI);
                normDagger.Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
                normDagger.usesLocalNPCImmunity = true;
                normDagger.penetrate += player.Calamity().StealthStrikeAvailable()
                    ? 1
                    : 0;

                velocity = new Vector2(speedX, speedY);
                velocity = velocity.RotatedBy(MathHelper.ToRadians(15));

                Projectile specDagger = Projectile.NewProjectileDirect(position, velocity,
                    ModContent.ProjectileType<DecreeDaggerProj>(), damage, knockBack, player.whoAmI);
                specDagger.Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
                specDagger.usesLocalNPCImmunity = true;
                return false;
            }

            Vector2 newVelocity = new Vector2(speedX, speedY);
            newVelocity = newVelocity.RotatedBy(MathHelper.ToRadians(15));

            Main.projectile[
                Projectile.NewProjectile(position, newVelocity, ModContent.ProjectileType<DecreeDaggerProj>(), damage,
                    knockBack, player.whoAmI)].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
            return true;
        }
    }
}