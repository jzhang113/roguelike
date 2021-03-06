﻿using Roguelike.Actions;
using Roguelike.Commands;
using Roguelike.Core;
using Roguelike.Interfaces;
using Roguelike.Systems;
using System;
using System.Collections.Generic;

namespace Roguelike.Actors
{
    [Serializable]
    internal class Titan : Actor, IEquipped
    {
        public EquipmentHandler Equipment { get; }

        private readonly IList<IAction> _attacks;
        private int _current;

        public override int Size { get; } = 2;

        public Titan(ActorParameters parameters) : base(parameters, Swatch.DbBlood, (char)0x1054)
        {
            Equipment = new EquipmentHandler();
            Facing = Direction.SE;
            _attacks = new List<IAction>()
            {
                new DamageAction(50, new TargetZone(TargetShape.Range)),
                new DamageAction(50, new TargetZone(TargetShape.Range)),
                new DamageAction(100, new TargetZone(TargetShape.Self, 1, 2), 240, 240),
            };
            _current = 0;
        }

        public override ICommand GetAction()
        {
            if (Game.Map.PlayerMap[Loc.X, Loc.Y] <= 2)
            {
                // in attack range
                // TODO: better decision of when to use large attacks
                Loc dir = Utils.Distance.GetNearestDirection(Game.Player.Loc, Loc);
                IAction action = _attacks[_current];
                IEnumerable<Loc> targets = action.Area.GetTilesInRange(this, Loc + dir);
                ICommand command = new DelayActionCommand(this, action, targets);

                if (++_current >= _attacks.Count)
                    _current = 0;

                return command;
            }
            else if (Game.Map.PlayerMap[Loc.X, Loc.Y] < Parameters.Awareness)
            {
                // in range, chase
                // TODO: don't chase if not alerted
                LocCost nextMove = Game.Map.MoveTowardsTarget(Loc, Game.Map.PlayerMap);
                return new MoveCommand(this, nextMove.Loc);
            }
            else
            {
                // out of range, sleep
                return new WaitCommand(this);
            }
        }
    }
}
