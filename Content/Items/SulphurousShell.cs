using System;
using CalamityMod.World;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;

namespace CataclysmMod.Content.Items
{
    public class SulphurousShell : CataclysmItem
    {
        public bool inUse;
        public int use;

        public override void SetDefaults()
        {
            item.useTurn = true;
            item.width = item.height = 20;
            item.useStyle = ItemUseStyleID.HoldingUp;
            item.useTime = item.useAnimation = 90;
            item.UseSound = SoundID.Item6;
            item.rare = ItemRarityID.Blue;
            item.value = Item.sellPrice(gold: 1);
        }

        public override bool UseItem(Player player)
        {
            inUse = true;

            return true;
        }

        public override void HoldItem(Player player)
        {
            if (!inUse)
                return;

            for (int i = 0; i < 2; i++)
            {
                if (Main.rand.Next(3) != 0)
                    continue;

                Vector2 spawnPos = player.Bottom +
                                   Vector2.UnitY.RotatedBy(player.itemAnimation * (Math.PI * 2f) / 30f) *
                                   new Vector2(15f, 0f);

                Dust wateryDust = Dust.NewDustPerfect(
                    spawnPos, 
                    DustID.BlueCrystalShard);
                wateryDust.velocity.Y = -4.5f;
                wateryDust.velocity.X *= 1.5f;
                wateryDust.scale = 0.8f;
                wateryDust.alpha = 130;
                wateryDust.noGravity = true;
                wateryDust.fadeIn = 1.1f;
            }

            if (++use < 90)
                return;

            switch (Main.netMode)
            {
                // TODO: Netcode.
                case NetmodeID.SinglePlayer:
                    TeleportSulphurousShell(player);
                    break;
                case NetmodeID.MultiplayerClient when player.whoAmI == Main.myPlayer:
                    try
                    {
                        TeleportSulphurousShell(player);
                    }
                    catch (Exception e)
                    {
                        Main.NewText(
                            "Exception thrown whilst attempting to teleport you to the Sulphurous Sea! Error has been logged.");
                        CataclysmMod.Instance.Logger.Error("Error thrown whilst attempting to use the Sulphurous Shell.",
                            e);
                    }

                    break;
            }

            use = 0;
            inUse = false;
        }

        private static void TeleportSulphurousShell(Player player)
        {
            Vector2 specialPos = Vector2.Zero;
            int abyssSide = -CalamityWorld.abyssSide.ToDirectionInt();
            int startX = CalamityWorld.abyssSide ? 200 : Main.maxTilesX - 200;
            bool specialTeleport = true;

            if (!RequestSulphurousTeleportPosition(player, -abyssSide, startX, out Point landingPoint))
            {
                specialTeleport = false;
                startX = !CalamityWorld.abyssSide ? 50 : Main.maxTilesX - 50;

                if (RequestSulphurousTeleportPosition(player, abyssSide, startX, out landingPoint))
                    specialTeleport = true;
            }

            if (specialTeleport)
                specialPos = landingPoint.ToWorldCoordinates(8f, 16f) - new Vector2(player.width / 2, player.height);

            if (specialTeleport)
            {
                player.Teleport(specialPos, 5);

                player.velocity = Vector2.Zero;

                if (Main.netMode != NetmodeID.Server)
                    return;

                RemoteClient.CheckSection(player.whoAmI, player.position);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, specialPos.X, specialPos.Y, 5);
            }
            else
            {
                Vector2 position = player.position;
                player.Teleport(position, 5);
                player.velocity = Vector2.Zero;

                if (Main.netMode != NetmodeID.Server)
                    return;

                RemoteClient.CheckSection(player.whoAmI, player.position);
                NetMessage.SendData(MessageID.Teleport, -1, -1, null, 0, player.whoAmI, position.X, position.Y, 5, 1);
            }
        }

        private static bool RequestSulphurousTeleportPosition(Player player, int crawlOffsetX, int startX,
            out Point landingPoint)
        {
            landingPoint = default;

            Point point = new Point(startX, 50);
            Vector2 halfPlayer = new Vector2(player.width * 0.5f, player.height);
            bool tileIsSloped = WorldGen.SolidOrSlopedTile(Main.tile[point.X, point.Y]);

            int loop1 = 0;
            int loop2 = 0;

            while (loop1 < 10000 && loop2 < 10000)
            {
                loop1++;

                Tile topTile = Main.tile[point.X, point.Y];
                Tile bottomTile = Main.tile[point.X, point.Y + 1];
                bool slopedOrLiquidTop = WorldGen.SolidOrSlopedTile(topTile) || topTile.liquid > 0;
                bool slopedOrLiquidBottom = WorldGen.SolidOrSlopedTile(bottomTile) || bottomTile.liquid > 0;

                if (IsInSolidTilesExtended(new Vector2(point.X * 16 + 8, point.Y * 16 + 15) - halfPlayer,
                    player.velocity, player.width, player.height, (int) player.gravDir))
                {
                    if (tileIsSloped)
                        point.Y += 1;
                    else
                        point.Y += -1;

                    continue;
                }

                if (slopedOrLiquidTop)
                {
                    if (tileIsSloped)
                        point.Y += 1;
                    else
                        point.Y += -1;

                    continue;
                }

                tileIsSloped = false;

                if (!IsInSolidTilesExtended(new Vector2(point.X * 16 + 8, point.Y * 16 + 15 + 16) - halfPlayer,
                        player.velocity, player.width, player.height, (int) player.gravDir) && !slopedOrLiquidBottom &&
                    point.Y < Main.worldSurface)
                {
                    point.Y += 1;
                    continue;
                }

                if (bottomTile.liquid > 0)
                {
                    point.X += crawlOffsetX;
                    loop2++;
                    continue;
                }

                if (TileIsDangerous(point.X, point.Y))
                {
                    point.X += crawlOffsetX;
                    loop2++;
                    continue;
                }

                if (TileIsDangerous(point.X, point.Y + 1))
                {
                    point.X += crawlOffsetX;
                    loop2++;
                    continue;
                }

                if (point.Y >= 40)
                    break;

                point.Y += 1;
            }

            if (loop1 == 5000 || loop2 >= 400 || !WorldGen.InWorld(point.X, point.Y, 40))
                return false;

            landingPoint = point;

            return true;
        }

        private static bool IsInSolidTilesExtended(Vector2 testPosition, Vector2 playerVelocity, int width, int height,
            int gravDir)
        {
            if (Collision.LavaCollision(testPosition, width, height) ||
                Collision.HurtTiles(testPosition, playerVelocity, width, height).Y > 0f ||
                Collision.SolidCollision(testPosition, width, height))
                return true;

            Vector2 vector = Vector2.UnitX * 16f;

            if (Collision.TileCollision(testPosition - vector, vector, width, height, false, false, gravDir) != vector)
                return true;

            vector = -Vector2.UnitX * 16f;

            if (Collision.TileCollision(testPosition - vector, vector, width, height, false, false, gravDir) != vector)
                return true;

            vector = Vector2.UnitY * 16f;

            if (Collision.TileCollision(testPosition - vector, vector, width, height, false, false, gravDir) != vector)
                return true;

            vector = -Vector2.UnitY * 16f;

            return Collision.TileCollision(testPosition - vector, vector, width, height, false, false, gravDir) !=
                   vector;
        }

        private static bool TileIsDangerous(int x, int y)
        {
            Tile tile = Main.tile[x, y];
            if (tile.liquid > 0 && tile.lava())
                return true;

            if (tile.wall == 87 && y > Main.worldSurface && !NPC.downedPlantBoss)
                return true;

            return Main.wallDungeon[tile.wall] && y > Main.worldSurface && !NPC.downedBoss3;
        }
    }
}