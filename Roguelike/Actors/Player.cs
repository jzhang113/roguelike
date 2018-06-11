﻿using Roguelike.Commands;
using Roguelike.Core;
using Roguelike.Interfaces;
using Roguelike.Systems;
using System;

namespace Roguelike.Actors
{
    class Player : Actor
    {
        public Player()
        {
            Awareness = 10;
            Name = "Player";
            Color = Colors.Player;
            Symbol = '@';

            HP = 100;
            SP = 50;
            MP = 50;
        }

        public override ICommand Act() => InputHandler.HandleInput();

        public override void TriggerDeath() => Game.GameOver();
    }
}
