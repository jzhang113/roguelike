﻿using Roguelike.Actions;
using Roguelike.Actors;
using Roguelike.Animations;
using Roguelike.Core;
using Roguelike.Interfaces;
using Roguelike.Statuses;
using Roguelike.Systems;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.Commands
{
    internal class MoveCommand : ICommand
    {
        public Actor Source { get; }
        public int EnergyCost => Data.Constants.FULL_TURN;
        public IAnimation Animation => null;

        private readonly int _newX;
        private readonly int _newY;
        private readonly Tile _tile;

        public MoveCommand(Actor source, int x, int y)
        {
            Source = source;
            _newX = x;
            _newY = y;
            _tile = Game.Map.Field[_newX, _newY];
        }

        public RedirectMessage Validate()
        {
            // Cancel out of bound moves.
            if (!Game.Map.Field.IsValid(_newX, _newY))
                return new RedirectMessage(false, new WaitCommand(Source));

            // Don't walk into walls, unless the Actor is currently phasing or we are already
            // inside a wall (to prevent getting stuck).
            if (_tile.IsWall && !Source.StatusHandler.TryGetStatus(StatusType.Phasing, out _)
                && !Game.Map.Field[Source.X, Source.Y].IsWall)
            {
                // Don't penalize the player for walking into walls, but monsters should wait if 
                // they will walk into a wall.
                if (Source is Player)
                    return new RedirectMessage(false);
                else
                    return new RedirectMessage(false, new WaitCommand(Source));
            }

            if (Game.Map.TryGetDoor(_tile.X, _tile.Y, out Door door))
            {
                // HACK: need an open door command
                if (!door.IsOpen)
                {
                    door.Open();
                    return new RedirectMessage(false, new WaitCommand(door, 120));
                }
            }

            // Check if the destination is already occupied.
            if (Game.Map.TryGetActor(_tile.X, _tile.Y, out Actor target))
            {
                if (target == Source)
                    return new RedirectMessage(false, new WaitCommand(Source));

                IAction attack = Source.GetBasicAttack();
                IEnumerable<Tile> targets = attack.Area.GetTilesInRange(Source, _tile.X, _tile.Y);
                return new RedirectMessage(false, new DelayActionCommand(Source, attack, targets));
            }

            return new RedirectMessage(true);
        }

        public void Execute()
        {
            Game.MessageHandler.AddMessage(
                $"{Source.Name} moved to {_newX}, {_newY} and is at {Source.Energy} energy",
                MessageLevel.Verbose);

            Source.Facing = Utils.Distance.GetNearestDirection(_newX, _newY, Source.X, Source.Y);

            if (Source is IEquipped equipped)
                equipped.Equipment.PrimaryWeapon?.AttackReset();

            if (Source is Player)
            {
                // TODO: better handling of move over popups
                if (Game.Map.TryGetStack(_newX, _newY, out InventoryHandler stack) && !stack.IsEmpty())
                {
                    Game.MessageHandler.AddMessage(stack.Count == 1
                        ? $"You see {stack.First()} here."
                        : "You see several items here.");
                }

                if (Game.Map.TryGetExit(_newX, _newY, out Exit exit))
                {
                    Game.MessageHandler.AddMessage($"You see an exit to {exit.Destination}.");
                }
            }

            Game.Map.SetActorPosition(Source, _newX, _newY);

            if (Source is Player)
                Game.Map.Refresh();
        }
    }
}