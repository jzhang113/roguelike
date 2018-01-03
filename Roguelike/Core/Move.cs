﻿using System.Collections.Generic;
using Roguelike.Systems;

namespace Roguelike.Core
{
    class Move
    {
        public static WeightedPoint N { get; } = new WeightedPoint(0, -1, 1);
        public static WeightedPoint E { get; } = new WeightedPoint(1, 0, 1);
        public static WeightedPoint S { get; } = new WeightedPoint(0, 1, 1);
        public static WeightedPoint W { get; } = new WeightedPoint(-1, 0, 1);
        public static WeightedPoint NE { get; } = new WeightedPoint(1, -1, 1.4f);
        public static WeightedPoint SE { get; } = new WeightedPoint(1, 1, 1.4f);
        public static WeightedPoint SW { get; } = new WeightedPoint(-1, 1, 1.4f);
        public static WeightedPoint NW { get; } = new WeightedPoint(-1, -1, 1.4f);

        public static IEnumerable<WeightedPoint> Directions = new WeightedPoint[]{ N, NE, E, SE, S, SW, W, NW };
    }
}
