﻿using Roguelike.Data;
using Roguelike.Interfaces;
using System;

namespace Roguelike.Core
{
    [Serializable]
    public class Tile
    {
        public int X { get; }
        public int Y { get; }

        public float Light
        {
            get => _light;
            internal set
            {
                if (value < 0)
                    _light = 0;
                else if (value > 1)
                    _light = 1;
                else
                    _light = value;
            }
        }

        public TerrainType Type
        {
            get => _type;
            internal set
            {
                _type = value;
                DrawingComponent = value.ToDrawable();
            }
        }

        public int Fuel { get; internal set; }
        public bool IsOccupied { get; internal set; }
        public bool IsExplored { get; internal set; }
        public bool BlocksLight { get; internal set; }
        public bool LosExists { get; internal set; }
#if DEBUG
        public int Debug { get; set; }
#endif

        public bool IsVisible => LosExists && Light > Constants.MIN_VISIBLE_LIGHT_LEVEL;
        public bool IsWall => Type == TerrainType.Wall;
        public bool IsWalkable => !IsWall && !IsOccupied;
        public bool IsLightable => !IsWall && !BlocksLight;

        public Drawable DrawingComponent { get; private set; }

        private float _light;
        private TerrainType _type;

        public Tile(int x, int y, TerrainType type)
        {
            X = x;
            Y = y;
            Type = type;

            Fuel = 10;
        }
    }
}