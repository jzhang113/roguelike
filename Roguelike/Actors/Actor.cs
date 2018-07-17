﻿using Roguelike.Actions;
using Roguelike.Commands;
using Roguelike.Core;
using Roguelike.Interfaces;
using Roguelike.Systems;
using System;

namespace Roguelike.Actors
{
    [Serializable]
    public class Actor : ISchedulable
    {
        public string Name => Parameters.Type;
        public bool BlocksLight { get; set; }

        public int Hp { get; set; }
        public int Mp { get; set; }
        public int Sp { get; set; }

        public ActorState State { get; set; }

        public int Energy { get; set; }
        public int RefreshRate { get; set; }

        public InventoryHandler Inventory { get; }
        public Drawable DrawingComponent { get; }
        public ActorParameters Parameters { get; }

        public int X
        {
            get => DrawingComponent.X;
            set => DrawingComponent.X = value;
        }

        public int Y
        {
            get => DrawingComponent.Y;
            set => DrawingComponent.Y = value;
        }

        public bool IsDead => Hp < 0;

        public Actor(ActorParameters parameters, RLNET.RLColor color, char symbol)
        {
            Parameters = parameters;
            Hp = Parameters.MaxHp;
            Sp = Parameters.MaxSp;
            Mp = Parameters.MaxMp;

            Energy = 0;
            RefreshRate = Data.Constants.DEFAULT_REFRESH_RATE;
            Inventory = new InventoryHandler();

            DrawingComponent = new Drawable
            {
                Color = color,
                Symbol = symbol
            };

            BlocksLight = true;
        }

        public virtual void TriggerDeath()
        {
            Game.Map.RemoveActor(this);
            Game.EventScheduler.RemoveActor(this);

            if (Game.Map.Field[X, Y].IsVisible)
            {
                Game.MessageHandler.AddMessage($"{Name} dies");
                Game.Map.Refresh();
            }
        }

        public virtual ICommand Act()
        {
            return SimpleAI.GetAction(this);
        }

        public virtual IAction GetBasicAttack()
        {
            return new DamageAction(100, new TargetZone(TargetShape.Directional));
        }

        public int TakeDamage(int power)
        {
            Hp -= power;
            return power;
        }
        public int TakeHealing(int power)
        {
            int restore = Parameters.MaxHp - Hp;
            restore = (restore > 0) ? restore : 0;

            if (restore > power)
            {
                Hp += power;
                return power;
            }
            else
            {
                Hp += restore;
                return restore;
            }
        }

        // Returns the Schedulable with higher energy. If there is a tie with another Actor, the
        // Actor closer to the Player gets to move first to preserve proper pathing. However, 
        // since the Player is also an Actor, this does not apply to cases when other is Player.
        public int CompareTo(ISchedulable other)
        {
            int energyDiff = Energy - other.Energy;

            if (energyDiff != 0 || !(other is Actor otherActor))
                return energyDiff;

            float distPlayer = Game.Map.PlayerMap[X, Y];
            float otherDistPlayer = Game.Map.PlayerMap[otherActor.X, otherActor.Y];

            // if we haven't discovered one of the actors, don't change the order
            return float.IsNaN(distPlayer) || float.IsNaN(otherDistPlayer)
                ? 0
                : (int) (otherDistPlayer - distPlayer);
        }
    }
}
