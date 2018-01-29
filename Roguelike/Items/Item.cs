﻿using RLNET;
using Roguelike.Interfaces;

namespace Roguelike.Items
{
    class Item : Drawable, IObject
    {
        public int AttackSpeed { get; set; }
        public int Damage { get; set; }
        public int MeleeRange { get; set; }
        public int ThrowRange { get; set; }

        public IMaterial Material { get; set; }
        public IActor Carrier { get; set; }
        public override RLColor Color { get; set; }
        public override char Symbol { get; set; }
        public override int X { get; set; }
        public override int Y { get; set; }

        public virtual void Apply()
        {
            Game.MessageHandler.AddMessage("Nothing happens.");
        }

        public virtual void Attack()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Consume()
        {
            Game.MessageHandler.AddMessage("That would be unhealthy.");
        }

        public virtual void Equip()
        {
            Game.MessageHandler.AddMessage("You cannot equip this.");
        }

        public virtual void Throw()
        {
            throw new System.NotImplementedException();
        }

        public virtual void Unequip()
        {
            Game.MessageHandler.AddMessage("You cannot take it off.");
        }
    }
}