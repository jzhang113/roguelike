﻿using Roguelike.Interfaces;
using Roguelike.World;
using System;

namespace Roguelike.Core
{
    [Serializable]
    public class Exit
    {
        public LevelId Destination { get; }
        public Drawable DrawingComponent { get; }
        public Loc Loc { get; set; }

        public Exit(in LevelId destination, char symbol)
        {
            Destination = destination;
            DrawingComponent = new Drawable(Colors.Exit, symbol, true);
        }
    }
}
