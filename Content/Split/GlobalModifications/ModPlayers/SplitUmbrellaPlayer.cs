using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using CataclysmMod.Common.ModCompatibility;
using CataclysmMod.Common.References;
using CataclysmMod.Content.Default.GlobalModifications;
using CataclysmMod.Content.Split.Projectiles;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace CataclysmMod.Content.Split.GlobalModifications.ModPlayers
{
    [ModDependency("Split")]
    public class SplitUmbrellaPlayer : CataclysmPlayer
    {
        public override void PreUpdate()
        {
            base.PreUpdate();

            bool PlayerHoldingUmbrella() => player.HeldItem.type == ItemID.Umbrella;

            int GetValidCount() => PlayerHoldingUmbrella() ? 1 : 0;

            IEnumerable<EntityReference<Projectile>> GetUmbrellaProjectiles(int owner)
            {
                Projectile[] projectiles = Main.projectile.Where(x =>
                        x.owner.Equals(owner) && x.type.Equals(ModContent.ProjectileType<PlayerUmbrellaProjectile>()) && x.active)
                    .ToArray();
                return EntityReference<Projectile>.FormReferenceCollection(projectiles);
            }

            foreach (EntityReference<Projectile> projectile in GetUmbrellaProjectiles(Main.myPlayer))
                if (GetUmbrellaProjectiles(Main.myPlayer).Count() > GetValidCount() || GetValidCount() == 0)
                    projectile.Execute(ref Main.projectile, x => x.Kill());

            if (PlayerHoldingUmbrella() && !GetUmbrellaProjectiles(Main.myPlayer).Any())
                Projectile.NewProjectile(player.position, Vector2.Zero,
                    ModContent.ProjectileType<PlayerUmbrellaProjectile>(), 0, 0f, Main.myPlayer);
        }
    }
}