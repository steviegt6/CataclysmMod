using CataclysmMod.Content.Default.Projectiles;
using CataclysmMod.Core.ModCompatibility;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace CataclysmMod.Content.Split.Projectiles
{
    [ModDependency("Split")]
    public class PlayerUmbrellaProjectile : CataclysmProjectile
    {
        public override string Texture => "Terraria/Projectile_1";

        public override bool PreDraw(SpriteBatch spriteBatch, Color lightColor) => false;

        public override void SetDefaults()
        {
            base.SetDefaults();

            projectile.Size = new Vector2(46f, 20f);
        }

        public override void AI()
        {
            base.AI();

            Player player = Main.player[projectile.owner];
            projectile.position = player.position - new Vector2(6f, 30f);

            if (player.direction == -1)
                projectile.position.X -= 14f;
        }
    }
}