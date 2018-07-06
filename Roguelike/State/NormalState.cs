﻿using RLNET;
using Roguelike.Actions;
using Roguelike.Actors;
using Roguelike.Commands;
using Roguelike.Core;
using Roguelike.Systems;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Roguelike.State
{
    class NormalState : IState
    {
        private static readonly Lazy<NormalState> _instance = new Lazy<NormalState>(() => new NormalState());
        public static NormalState Instance => _instance.Value;

        private NormalState()
        {
        }

        public ICommand HandleKeyInput(RLKeyPress keyPress)
        {
            if (keyPress == null)
                return null;

            Player player = Game.Player;
            IAction ability = null;

            if (keyPress.Shift)
                ability = player.Equipment.PrimaryWeapon.GetAbility(0);
            else if (keyPress.Alt)
                ability = player.Equipment.PrimaryWeapon.GetAbility(1);
            else if (keyPress.Control)
                ability = player.Equipment.PrimaryWeapon.GetAbility(2);

            return ability != null
                ? ResolveAttackInput(keyPress, player, ability)
                : ResolveInput(keyPress, player);
        }

        // ReSharper disable once CyclomaticComplexity
        private static ICommand ResolveInput(RLKeyPress keyPress, Actor player)
        {
            switch (keyPress.Key)
            {
                #region Movement Keys
                case RLKey.Left:
                case RLKey.Keypad4:
                case RLKey.H:
                    return new MoveCommand(player, player.X + Direction.W.X, player.Y);
                case RLKey.Down:
                case RLKey.Keypad2:
                case RLKey.J:
                    return new MoveCommand(player, player.X, player.Y + Direction.S.Y);
                case RLKey.Up:
                case RLKey.Keypad8:
                case RLKey.K:
                    return new MoveCommand(player, player.X, player.Y + Direction.N.Y);
                case RLKey.Right:
                case RLKey.Keypad6:
                case RLKey.L:
                    return new MoveCommand(player, player.X + Direction.E.X, player.Y);
                case RLKey.Keypad7:
                case RLKey.Y:
                    return new MoveCommand(player, player.X + Direction.NW.X, player.Y + Direction.NW.Y);
                case RLKey.Keypad9:
                case RLKey.U:
                    return new MoveCommand(player, player.X + Direction.NE.X, player.Y + Direction.NE.Y);
                case RLKey.Keypad1:
                case RLKey.B:
                    return new MoveCommand(player, player.X + Direction.SW.X, player.Y + Direction.SW.Y);
                case RLKey.Keypad3:
                case RLKey.N:
                    return new MoveCommand(player, player.X + Direction.SE.X, player.Y + Direction.SE.Y);
                case RLKey.Keypad5:
                case RLKey.Period:
                    return new WaitCommand(player);
                #endregion

                case RLKey.Comma:
                    // TODO: only grabs top item
                    if (Game.Map.TryGetStack(player.X, player.Y, out InventoryHandler stack))
                        return new PickupCommand(player, stack);
                    else
                        return null;
                case RLKey.BackSlash:
                    // HACK: Ad-hoc input handling
                    if (Game.Map.TryChangeLocation(player, out World.LevelId destination))
                        return new ChangeLevelCommand(player, destination);
                    else
                        return null;
                case RLKey.A:
                    Game.StateHandler.PushState(ApplyState.Instance);
                    return null;
                case RLKey.D:
                    Game.StateHandler.PushState(DropState.Instance);
                    return null;
                case RLKey.E:
                    Game.StateHandler.PushState(EquipState.Instance);
                    return null;
                case RLKey.I:
                    Game.StateHandler.PushState(InventoryState.Instance);
                    return null;
                case RLKey.T:
                    Game.StateHandler.PushState(UnequipState.Instance);
                    return null;
                case RLKey.O:
                    Game.StateHandler.PushState(AutoexploreState.Instance);
                    return null;
                case RLKey.Q:
                    // NOTE: Player actually gets a double turn when using the hook, but it seems ok.
                    IAction hookAction = new HookAction(100);
                    Game.StateHandler.PushState(new TargettingState(
                        Game.Player,
                        hookAction,
                        returnTarget => new ActionCommand(Game.Player, hookAction, returnTarget)));
                    return null;
                case RLKey.R:
                    Game.NewGame();
                    return null;
                case RLKey.Escape:
                    Game.Exit();
                    return null;
                default: return null;
            }
        }

        // ReSharper disable once CyclomaticComplexity
        private static ICommand ResolveAttackInput(RLKeyPress keyPress, Actor player, IAction ability)
        {
            switch (keyPress.Key)
            {
                case RLKey.Left:
                case RLKey.Keypad4:
                case RLKey.H:
                    return new ActionCommand(player, ability, Game.Map.Field[player.X + Direction.W.X, player.Y]);
                case RLKey.Down:
                case RLKey.Keypad2:
                case RLKey.J:
                    return new ActionCommand(player, ability, Game.Map.Field[player.X, player.Y + Direction.S.Y]);
                case RLKey.Up:
                case RLKey.Keypad8:
                case RLKey.K:
                    return new ActionCommand(player, ability, Game.Map.Field[player.X, player.Y + Direction.N.Y]);
                case RLKey.Right:
                case RLKey.Keypad6:
                case RLKey.L:
                    return new ActionCommand(player, ability, Game.Map.Field[player.X + Direction.E.X, player.Y]);
                case RLKey.Keypad7:
                case RLKey.Y:
                    return new ActionCommand(player, ability,
                        Game.Map.Field[player.X + Direction.NW.X, player.Y + Direction.NW.Y]);
                case RLKey.Keypad9:
                case RLKey.U:
                    return new ActionCommand(player, ability,
                        Game.Map.Field[player.X + Direction.NE.X, player.Y + Direction.NE.Y]);
                case RLKey.Keypad1:
                case RLKey.B:
                    return new ActionCommand(player, ability,
                        Game.Map.Field[player.X + Direction.SW.X, player.Y + Direction.SW.Y]);
                case RLKey.Keypad3:
                case RLKey.N:
                    return new ActionCommand(player, ability,
                        Game.Map.Field[player.X + Direction.SE.X, player.Y + Direction.SE.Y]);
                default: return null;
            }
        }

        public ICommand HandleMouseInput(RLMouse mouse)
        {
            //    map.ClearHighlight();
            //    Terrain current = map.Field[mousePos.X, mousePos.Y];

            //        // TODO: Path may end up broken because an enemy is in the way.
            //        IEnumerable<WeightedPoint> path = map.GetPathToPlayer(mousePos.X, mousePos.Y).Reverse();
            //        bool exploredPathExists = false;

            //        foreach (WeightedPoint p in path)
            //        {
            //            if (!exploredPathExists)
            //                exploredPathExists = true;

            //            if (!map.Field[p.X, p.Y].IsExplored)
            //            {
            //                exploredPathExists = false;
            //                break;
            //            }

            //            map.Highlight[p.X, p.Y] = RLColor.Red;
            //        }

            //        if (current.IsWalkable && exploredPathExists)
            //            map.Highlight[mousePos.X, mousePos.Y] = RLColor.Red;
            //    

            //    //if (_console.Mouse.GetLeftClick())
            //    //{
            //    //    List<IAction> moves = new List<IAction>();

            //    //    foreach (WeightedPoint p in path)
            //    //        moves.Add(new MoveAction(new TargetZone(TargetShape.Range, (p.X, p.Y))));

            //    //    return new AttackCommand(player, new ActionSequence(100, moves));
            //    //}
            //    if (map.TryGetActor(mousePos.X, mousePos.Y, out Actor displayActor))
            //        LookHandler.DisplayActor(displayActor);

            //    if (map.TryGetItem(mousePos.X, mousePos.Y, out ItemCount displayItem))
            //        LookHandler.DisplayItem(displayItem);

            //    LookHandler.DisplayTerrain(map.Field[mousePos.X, mousePos.Y]);
            return null;
        }

        public void Update()
        {
            ICommand command = Game.StateHandler.HandleInput();
            if (command == null)
                return;

            if (EventScheduler.Execute(Game.Player, command))
            {
                Game.ForceRender();

                if (command.Animation != null)
                    Game.StateHandler.PushState(new AnimationState(command.Animation));

                Game.EventScheduler.Run();
            }
        }

        public void Draw()
        {
            Game.Map.ClearHighlight();
            Game.MapConsole.Clear(0, RLColor.Black, Colors.TextHeading, 0);
            Game.Map.Draw(Game.MapConsole);
            RLConsole.Blit(Game.MapConsole, 0, 0, Game.Config.MapView.Width, Game.Config.MapView.Height, Game.RootConsole, 0, Game.Config.MessageView.Height);
        }
    }
}