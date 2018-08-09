﻿using Roguelike.Core;
using Roguelike.Interfaces;

namespace Roguelike.Items
{
    public class Armor : Item, IEquippable
    {
        public ArmorType Type { get; }

        public Armor()
        {
        }

        public Armor(ItemParameter parameters, RLNET.RLColor color, ArmorType type) : base(parameters, color, '[')
        {
            Type = type;
        }

        public Armor(Armor other) : base(other)
        {
            Type = other.Type;
        }

        public void Equip(IEquipped equipped)
        {
            System.Diagnostics.Debug.Assert(equipped != null);

            if (equipped.Equipment.Armor[Type] != null)
            {
                Game.MessageHandler.AddMessage(
                    $"You are already wearing a {equipped.Equipment.Armor[Type]}!");
                return;
            }

            equipped.Equipment.Equip(this);
            Game.MessageHandler.AddMessage($"You put on the {Name}.");
        }

        public void Unequip(IEquipped equipped)
        {
            System.Diagnostics.Debug.Assert(equipped != null);

            equipped.Equipment.Unequip(this);
            Game.MessageHandler.AddMessage($"You take off the {Name}.");
        }

        public override Item DeepClone()
        {
            return new Armor(this);
        }
    }
}
