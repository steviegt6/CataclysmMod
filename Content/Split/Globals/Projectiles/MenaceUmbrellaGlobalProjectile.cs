#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Linq;
using CataclysmMod.Common.Addons;
using CataclysmMod.Content.Split.Projectiles;
using CataclysmMod.Core.Loading;
using Split.Projectiles.Hostile.Bosses.Menace;
using Terraria;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.Globals.Projectiles
{
    [AddonContent(typeof(SplitAddon))]
    public class MenaceUmbrellaGlobalProjectile : GlobalProjectile
    {
        public override void AI(Projectile projectile)
        {
            base.AI(projectile);

            bool MenaceRainProjectile(int type) => type == ModContent.ProjectileType<MenacingKnife>() ||
                                                   type == ModContent.ProjectileType<IceBlock>();

            foreach (Projectile proj in Main.projectile.Where(x =>
                         x.active && x.type.Equals(ModContent.ProjectileType<PlayerUmbrellaProjectile>())))
                if (MenaceRainProjectile(projectile.type) && proj.Hitbox.Intersects(projectile.Hitbox))
                    projectile.Kill();
        }
    }
}