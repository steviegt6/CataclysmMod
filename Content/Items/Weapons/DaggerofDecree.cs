using CalamityMod;
using CalamityMod.Items;
using CalamityMod.Items.Weapons.Rogue;
using CalamityMod.Projectiles.Rogue;
using CataclysmMod.Content.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Items.Weapons
{
    public class DaggerofDecree : RogueWeapon
    {
        public override string Texture => "CalamityMod/Items/Weapons/Rogue/CursedDagger";

        public override bool Autoload(ref string name) => CataclysmMod.Instance.Calamity != null;

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Dagger of Decree");
            Tooltip.SetDefault("Throws two daggers, with the top dagger having homing and splitting capabilities" +
                "\nStealth strikes cause the top dagger to split into two more on death," +
                "\ngives the bottom dagger higher pierce, " +
                "\nand both daggers become showered in cursed fireballs");
        }

        public override void SafeSetDefaults()
        {
            item.width = 24;
            item.height = 34;

            item.damage = 26;
            item.noMelee = true;
            item.noUseGraphic = true;
            item.useAnimation = 18;
            item.useTime = 18;
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

        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            if (player.Calamity().StealthStrikeAvailable())
            {
                int normDagger = Projectile.NewProjectile(position, new Vector2(speedX, speedY) * (player.Calamity().StealthStrikeAvailable() ? 1.25f : 1f), type, damage, knockBack, player.whoAmI);
                Main.projectile[normDagger].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
                Main.projectile[normDagger].usesLocalNPCImmunity = true;
                Main.projectile[normDagger].penetrate += player.Calamity().StealthStrikeAvailable() ? 1 : 0;

                int specDagger = Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(15)), ModContent.ProjectileType<DecreeDaggerProj>(), damage, knockBack, player.whoAmI);
                Main.projectile[specDagger].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();
                Main.projectile[specDagger].usesLocalNPCImmunity = true;

                return false;
            }
            else
                Main.projectile[Projectile.NewProjectile(position, new Vector2(speedX, speedY).RotatedBy(MathHelper.ToRadians(15)), ModContent.ProjectileType<DecreeDaggerProj>(), damage, knockBack, player.whoAmI)].Calamity().stealthStrike = player.Calamity().StealthStrikeAvailable();

            return true;
        }
    }
}