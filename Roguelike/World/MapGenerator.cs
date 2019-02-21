﻿using Pcg;
using Roguelike.Actions;
using Roguelike.Core;
using Roguelike.Data;
using Roguelike.Items;
using System;
using System.Collections.Generic;

namespace Roguelike.World
{
    public abstract class MapGenerator
    {
        protected int Width { get; }
        protected int Height { get; }
        protected PcgRandom Rand { get; }
        protected MapHandler Map { get; }

        protected IList<Room> RoomList { get; set; }
        protected ICollection<int>[] Adjacency { get; set; }

        private IEnumerable<LevelId> Exits { get; }

        protected MapGenerator(int width, int height, IEnumerable<LevelId> exits, PcgRandom random)
        {
            Width = width;
            Height = height;
            Exits = exits;
            Rand = random;
            Map = new MapHandler(width, height);
        }

        public MapHandler Generate()
        {
            CreateMap();
            ComputeClearance();

            PlaceActors();
            PlaceItems();
            PlaceStairs();

            return Map;
        }

        protected abstract void CreateMap();

        // Calculate and save how much space is around each square
        private void ComputeClearance()
        {
            for (int y = 0; y < Map.Height; y++)
            {
                for (int x = 0; x < Map.Width; x++)
                {
                    int d = 0;
                    while (true)
                    {
                        for (int c = 0; c <= d; c++)
                        {
                            if (Map.Field[x + c, y].IsWall || Map.Field[x, y + c].IsWall)
                                goto done;
                        }

                        d++;
                    }

                    done:
                    Map.Clearance[x, y] = d;
                }
            }
        }

        protected void CreateRoom(Room room)
        {
            for (int i = room.Left; i < room.Right; i++)
            {
                for (int j = room.Top; j < room.Bottom; j++)
                {
                    // Don't excavate the edges of the map
                    if (PointOnMap(i, j))
                        Map.Field[i, j].Type = TerrainType.Stone;
                }
            }
        }

        // Similar to CreateRoom, but doesn't leave a border.
        protected void CreateRoomWithoutBorder(Room room)
        {
            for (int i = room.Left; i <= room.Right; i++)
            {
                for (int j = room.Top; j <= room.Bottom; j++)
                {
                    if (PointOnMap(i, j))
                        Map.Field[i, j].Type = TerrainType.Stone;
                }
            }
        }

        protected void CreateHallway(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x1 - x2);
            int dy = Math.Abs(y1 - y2);

            if (x1 < x2)
            {
                if (y1 < y2)
                {
                    CreateRoom(new Room(x2 - dx, y2, dx + 1, 1));
                    CreateRoom(new Room(x1, y1, 1, dy + 1));
                }
                else
                {
                    CreateRoom(new Room(x1, y1, dx + 1, 1));
                    CreateRoom(new Room(x2, y2, 1, dy + 1));
                }
            }
            else
            {
                if (y1 < y2)
                {
                    CreateRoom(new Room(x2, y2, dx + 1, 1));
                    CreateRoom(new Room(x1, y1, 1, dy + 1));
                }
                else
                {
                    CreateRoom(new Room(x1 - dx, y1, dx + 1, 1));
                    CreateRoom(new Room(x2, y2, 1, dy + 1));
                }
            }
        }

        protected void PlaceDoors(Room room)
        {
            for (int i = room.Left; i < room.Right; i++)
            {
                if (i <= 0 || i >= Width - 1)
                    continue;

                if (room.Top > 1 && IsDoorLocation(i, room.Top - 1))
                {
                    Map.AddDoor(new Door
                    {
                        X = i,
                        Y = room.Top - 1
                    });
                }

                if (room.Bottom < Height - 1 && IsDoorLocation(i, room.Bottom))
                {
                    Map.AddDoor(new Door
                    {
                        X = i,
                        Y = room.Bottom
                    });
                }
            }

            for (int j = room.Top; j < room.Bottom; j++)
            {
                if (j <= 0 || j >= Height - 1)
                    continue;

                if (room.Left > 1 && IsDoorLocation(room.Left - 1, j))
                {
                    Map.AddDoor(new Door
                    {
                        X = room.Left - 1,
                        Y = j
                    });
                }

                if (room.Right < Width - 1 && IsDoorLocation(room.Right, j))
                {
                    Map.AddDoor(new Door
                    {
                        X = room.Right,
                        Y = j
                    });
                }
            }
        }

        private bool IsDoorLocation(int x, int y)
        {
            bool current = Map.Field[x, y].IsWall;
            bool left = Map.Field[x - 1, y].IsWall;
            bool right = Map.Field[x + 1, y].IsWall;
            bool up = Map.Field[x, y - 1].IsWall;
            bool down = Map.Field[x, y + 1].IsWall;

            if (current)
                return false;

            if (left && right && !up && !down)
                return true;

            if (!left && !right && up && down)
                return true;

            return false;
        }

        // HACK: ad-hoc placement code
        private void PlaceItems()
        {
            Weapon spear = new Weapon(
                new ItemParameter("spear", MaterialType.Wood)
                {
                    AttackSpeed = 240,
                    Damage = 200,
                    ThrowRange = 7
                }, Swatch.DbBlood)
            {
                X = Game.Player.X - 1,
                Y = Game.Player.Y - 1
            };
            Map.AddItem(spear);

            spear.Moveset = new Systems.MovesetHandler(new Systems.ActionNode(
                new Systems.ActionNode(
                    new Systems.ActionNode(
                        null,
                        null,
                        new MoveAction(new TargetZone(TargetShape.Range, 2)),
                        "Leap"),
                    null,
                    new DamageAction(300, new TargetZone(TargetShape.Range, 1)),
                    "Slam"),
                    new Systems.ActionNode(
                        null,
                        null,
                        new DamageAction(100, new TargetZone(TargetShape.Self, radius: 2)),
                        "Whirlwind"),
                new DamageAction(100, new TargetZone(TargetShape.Directional, 2)),
                "Strike"));

            IAction heal = new HealAction(100, new TargetZone(TargetShape.Self));

            Armor ha = new Armor(
                new ItemParameter("heavy armor", MaterialType.Iron)
                {
                    AttackSpeed = 1000,
                    Damage = 100,
                    MeleeRange = 1,
                    ThrowRange = 3
                }, Swatch.DbMetal, ArmorType.Armor)
            {
                X = Game.Player.X - 2,
                Y = Game.Player.Y - 3
            };
            Map.AddItem(ha);

            Map.AddItem(new Scroll(
                    new ItemParameter("scroll of magic missile", MaterialType.Paper),
                    new DamageAction(
                        200,
                        new TargetZone(TargetShape.Directional, range: 10)),
                    Swatch.DbSun)
                {
                    X = Game.Player.X - 1,
                    Y = Game.Player.Y - 2
                });

            Scroll healing = new Scroll(
                new ItemParameter("scroll of healing", MaterialType.Paper), heal, Swatch.DbGrass)
            {
                X = Game.Player.X + 1,
                Y = Game.Player.Y + 1
            };
            Map.AddItem(healing);

            Map.AddItem(new Scroll(
                    new ItemParameter("scroll of enchantment", MaterialType.Paper),
                    new EnchantAction(
                        new TargetZone(TargetShape.Range, range: 10)),
                    System.Drawing.Color.LightGreen)
                {
                    X = Game.Player.X - 1,
                    Y = Game.Player.Y
                });

            Map.AddItem(new Scroll(
                    new ItemParameter("scroll of fireball", MaterialType.Paper),
                    new DamageAction(
                        200,
                        new TargetZone(TargetShape.Range, range: 10, radius: 3)),
                    Swatch.DbBlood)
                {
                    X = Game.Player.X - 2,
                    Y = Game.Player.Y - 2
                });

            Item planks = new Item(
                    new ItemParameter("plank", MaterialType.Wood), Swatch.DbWood, '\\', 10)
            {
                X = Game.Player.X + 2,
                Y = Game.Player.Y + 2,
            };
            Item planks2 = new Item(planks, 10) { X = Game.Player.X + 3 };
            Item planks3 = new Item(planks, 10) { Y = Game.Player.Y + 3 };
            Item planks4 = new Item(planks, 10) { X = Game.Player.X + 3, Y = Game.Player.Y + 3 };
            Map.AddItem(planks);
            Map.AddItem(planks2);
            Map.AddItem(planks3);
            Map.AddItem(planks4);
        }

        // HACK: ad-hoc placement code
        private void PlaceActors()
        {
            do
            {
                Game.Player.X = Rand.Next(1, Width - 1);
                Game.Player.Y = Rand.Next(1, Height - 1);
            }
            while (!Map.Field[Game.Player.X, Game.Player.Y].IsWalkable);
            Map.AddActor(Game.Player);

            Actors.Titan titan = new Actors.Titan(new Actors.ActorParameters("Bob")
            {
                Awareness = 10,
                MaxHp = 250
            })
            {
                X = Game.Player.X - 4,
                Y = Game.Player.Y - 4
            };
            Map.AddActor(titan);

            Map.SetFire(Game.Player.X + 3, Game.Player.Y + 3);
        }

        private void PlaceStairs()
        {
            foreach (LevelId id in Exits)
            {
                LevelId current = Game.World.CurrentLevel;
                char symbol = '*';
                if (id.Name == current.Name)
                    symbol = id.Depth > current.Depth ? '>' : '<';

                Exit exit = new Exit(id, symbol);
                bool done = false;
                while (!Map.Field[exit.X, exit.Y].IsWalkable && !done)
                {
                    Map.GetExit(exit.X, exit.Y).Match(
                        some: _ =>
                        {
                            exit.X = Rand.Next(1, Width - 1);
                            exit.Y = Rand.Next(1, Height - 1);
                        },
                        none: () => done = true);
                }

                Map.AddExit(exit);
            }
        }

        protected bool PointOnMap(int x, int y)
        {
            return x > 0 && y > 0 && x < Width - 1 && y < Height - 1;
        }
    }
}
