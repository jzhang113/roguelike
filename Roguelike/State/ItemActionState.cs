﻿using BearLib;
using Roguelike.Commands;
using Roguelike.Core;
using Roguelike.Items;
using Roguelike.Utils;

namespace Roguelike.State
{
    internal abstract class ItemActionState : IState
    {
        public virtual ICommand HandleKeyInput(int key)
        {
            char keyChar = key.ToChar();
            if (!Game.Player.Inventory.TryGetKey(keyChar, out ItemCount itemCount))
            {
                Game.MessageHandler.AddMessage("No such item.");
                return null;
            }

            System.Diagnostics.Debug.Assert(itemCount.Item != null);
            System.Diagnostics.Debug.Assert(itemCount.Count > 0);
            return ResolveInput(itemCount);
        }

        public virtual ICommand HandleMouseInput(int x, int y, bool leftClick, bool rightClick)
        {
            // TODO do stuff and get the item selected
            return null;
        }

        protected abstract ICommand ResolveInput(ItemCount itemCount);

        public virtual void Update()
        {
            ICommand command = Game.StateHandler.HandleInput();
            if (command == null)
                return;

            Game.Player.NextCommand = command;
            Game.EventScheduler.Run();
            Game.StateHandler.PopState();

            //Q: can any item action events even have animations?
            if (command.Animation != null)
                Game.StateHandler.PushState(new AnimationState(command.Animation));
        }

        public virtual void Draw(LayerInfo layer)
        {
            // highlight draw borders
            Terminal.Color(Colors.HighlightColor);
            layer.DrawBorders(new BorderInfo
            {
                TopLeftChar = '╔', // 201
                TopRightChar = '╗', // 187
                BottomLeftChar = '╚', // 200
                BottomRightChar = '╝', // 188
                TopChar = '═', // 205
                BottomChar = '═',
                LeftChar = '║', // 186
                RightChar = '║'
            });
            layer.Print(-1, "[color=white][[INVENTORY]][/color][color=grass]EQUIPMENT]]");
        }
    }
}
