﻿using Roguelike.Actors;
using Roguelike.Systems;

namespace Roguelike.Core
{
    class Terrain
    {
        public bool IsWall { get; set; }
        public bool IsOccupied { get; set; }
        public bool IsWalkable { get => !IsWall && !IsOccupied; }

        public int MoveCost { get; set; }
        public Actor Unit { get; set; }
        public InventoryHandler ItemStack { get; set; }

        public WeightedPoint Position { get; }

        public Terrain(bool wall, int moveCost, int x, int y)
        {
            IsWall = wall;
            MoveCost = moveCost;
            Position = new WeightedPoint(x, y);
        }
    }
}