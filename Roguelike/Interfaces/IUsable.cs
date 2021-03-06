﻿using Roguelike.Core;
using System.Collections.Generic;
using Roguelike.Actions;
using Roguelike.Actors;

namespace Roguelike.Interfaces
{
    // Represents items that can be applied.
    public interface IUsable
    {
        // Action to perform.
        IAction ApplySkill { get; }

        // Perform the action.
        void Apply(Actor source, IEnumerable<Loc> targets);
    }
}
