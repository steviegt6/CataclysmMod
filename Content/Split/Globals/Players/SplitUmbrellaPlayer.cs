#region License
// Copyright (C) 2022 Tomat and Contributors, MIT License
#endregion

using System.Collections.Generic;
using System.Linq;
using CataclysmMod.Common.Addons;
using CataclysmMod.Content.Split.Projectiles;
using CataclysmMod.Core.Loading;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.Globals.Players
{
    [AddonContent(typeof(SplitAddon))]
    public class SplitUmbrellaPlayer : ModPlayer
    {
        public override void PreUpdate()
        {
            base.PreUpdate();

            bool PlayerHoldingUmbrella() => player.HeldItem.type == ItemID.Umbrella;

            int GetValidCount() => PlayerHoldingUmbrella() ? 1 : 0;

            IEnumerable<Projectile> GetUmbrellaProjectiles(int owner)
            {
                Projectile[] projectiles = Main.projectile.Where(x =>
                    x.owner.Equals(owner) && 
                    x.type.Equals(ModContent.ProjectileType<PlayerUmbrellaProjectile>()) &&
                    x.active
                ).ToArray();
                
                return projectiles;
            }

            foreach (Projectile projectile in GetUmbrellaProjectiles(Main.myPlayer))
                if (GetUmbrellaProjectiles(Main.myPlayer).Count() > GetValidCount() || GetValidCount() == 0)
                    projectile.Kill();

            if (PlayerHoldingUmbrella() && !GetUmbrellaProjectiles(Main.myPlayer).Any())
                Projectile.NewProjectile(
                    player.position,
                    Vector2.Zero,
                    ModContent.ProjectileType<PlayerUmbrellaProjectile>(),
                    0,
                    0f,
                    Main.myPlayer
                );
        }

        private static void Kill(ref Projectile projectile) => projectile.Kill();
    }
}