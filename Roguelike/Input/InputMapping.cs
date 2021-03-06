﻿using BearLib;
using System.Collections.Generic;

namespace Roguelike.Input
{
    internal static partial class InputMapping
    {
        private static readonly KeyMap _keyMap;

        static InputMapping()
        {
            _keyMap = new KeyMap()
            {
                NormalMap = new KeyMap.StateMap<NormalInput>()
                {
                    None = new Dictionary<int, NormalInput>()
                    {
                        [Terminal.TK_LEFT] =        NormalInput.MoveW,
                        [Terminal.TK_KP_4] =        NormalInput.MoveW,
                        [Terminal.TK_H] =           NormalInput.MoveW,
                        [Terminal.TK_DOWN] =        NormalInput.MoveS,
                        [Terminal.TK_KP_2] =        NormalInput.MoveS,
                        [Terminal.TK_J] =           NormalInput.MoveS,
                        [Terminal.TK_UP] =          NormalInput.MoveN,
                        [Terminal.TK_KP_8] =        NormalInput.MoveN,
                        [Terminal.TK_K] =           NormalInput.MoveN,
                        [Terminal.TK_RIGHT] =       NormalInput.MoveE,
                        [Terminal.TK_KP_6] =        NormalInput.MoveE,
                        [Terminal.TK_L] =           NormalInput.MoveE,
                        [Terminal.TK_KP_7] =        NormalInput.MoveNW,
                        [Terminal.TK_Y] =           NormalInput.MoveNW,
                        [Terminal.TK_KP_9] =        NormalInput.MoveNE,
                        [Terminal.TK_U] =           NormalInput.MoveNE,
                        [Terminal.TK_KP_1] =        NormalInput.MoveSW,
                        [Terminal.TK_B] =           NormalInput.MoveSW,
                        [Terminal.TK_KP_3] =        NormalInput.MoveSE,
                        [Terminal.TK_N] =           NormalInput.MoveSE,
                        [Terminal.TK_KP_5] =        NormalInput.Wait,
                        [Terminal.TK_PERIOD] =      NormalInput.Wait,
                        [Terminal.TK_COMMA] =       NormalInput.Get,
                        [Terminal.TK_BACKSLASH] =   NormalInput.ChangeLevel,
                        [Terminal.TK_A] =           NormalInput.OpenApply,
                        [Terminal.TK_D] =           NormalInput.OpenDrop,
                        [Terminal.TK_E] =           NormalInput.OpenEquip,
                        [Terminal.TK_I] =           NormalInput.OpenInventory,
                        [Terminal.TK_R] =           NormalInput.OpenUnequip,
                        [Terminal.TK_T] =           NormalInput.Throw,
                        [Terminal.TK_O] =           NormalInput.AutoExplore,
                        [Terminal.TK_ESCAPE] =      NormalInput.OpenMenu
                    },
                    Shift = new Dictionary<int, NormalInput>()
                    {
                        [Terminal.TK_LEFT] =        NormalInput.AttackW,
                        [Terminal.TK_KP_4] =        NormalInput.AttackW,
                        [Terminal.TK_H] =           NormalInput.AttackW,
                        [Terminal.TK_DOWN] =        NormalInput.AttackS,
                        [Terminal.TK_KP_2] =        NormalInput.AttackS,
                        [Terminal.TK_J] =           NormalInput.AttackS,
                        [Terminal.TK_UP] =          NormalInput.AttackN,
                        [Terminal.TK_KP_8] =        NormalInput.AttackN,
                        [Terminal.TK_K] =           NormalInput.AttackN,
                        [Terminal.TK_RIGHT] =       NormalInput.AttackE,
                        [Terminal.TK_KP_6] =        NormalInput.AttackE,
                        [Terminal.TK_L] =           NormalInput.AttackE,
                        [Terminal.TK_KP_7] =        NormalInput.AttackNW,
                        [Terminal.TK_Y] =           NormalInput.AttackNW,
                        [Terminal.TK_KP_9] =        NormalInput.AttackNE,
                        [Terminal.TK_U] =           NormalInput.AttackNE,
                        [Terminal.TK_KP_1] =        NormalInput.AttackSW,
                        [Terminal.TK_B] =           NormalInput.AttackSW,
                        [Terminal.TK_KP_3] =        NormalInput.AttackSE,
                        [Terminal.TK_N] =           NormalInput.AttackSE
                    }
                },
                TargettingMap = new KeyMap.StateMap<TargettingInput>()
                {
                    None = new Dictionary<int, TargettingInput>()
                    {
                        [Terminal.TK_LEFT] =        TargettingInput.MoveW,
                        [Terminal.TK_KP_4] =        TargettingInput.MoveW,
                        [Terminal.TK_H] =           TargettingInput.MoveW,
                        [Terminal.TK_DOWN] =        TargettingInput.MoveS,
                        [Terminal.TK_KP_2] =        TargettingInput.MoveS,
                        [Terminal.TK_J] =           TargettingInput.MoveS,
                        [Terminal.TK_UP] =          TargettingInput.MoveN,
                        [Terminal.TK_KP_8] =        TargettingInput.MoveN,
                        [Terminal.TK_K] =           TargettingInput.MoveN,
                        [Terminal.TK_RIGHT] =       TargettingInput.MoveE,
                        [Terminal.TK_KP_6] =        TargettingInput.MoveE,
                        [Terminal.TK_L] =           TargettingInput.MoveE,
                        [Terminal.TK_KP_7] =        TargettingInput.MoveNW,
                        [Terminal.TK_Y] =           TargettingInput.MoveNW,
                        [Terminal.TK_KP_9] =        TargettingInput.MoveNE,
                        [Terminal.TK_U] =           TargettingInput.MoveNE,
                        [Terminal.TK_KP_1] =        TargettingInput.MoveSW,
                        [Terminal.TK_B] =           TargettingInput.MoveSW,
                        [Terminal.TK_KP_3] =        TargettingInput.MoveSE,
                        [Terminal.TK_N] =           TargettingInput.MoveSE,
                        [Terminal.TK_TAB] =         TargettingInput.NextActor,
                        [Terminal.TK_ENTER] =       TargettingInput.Fire,
                        [Terminal.TK_KP_ENTER] =    TargettingInput.Fire
                    },
                    Shift = new Dictionary<int, TargettingInput>()
                    {
                        [Terminal.TK_LEFT] =        TargettingInput.JumpW,
                        [Terminal.TK_KP_4] =        TargettingInput.JumpW,
                        [Terminal.TK_H] =           TargettingInput.JumpW,
                        [Terminal.TK_DOWN] =        TargettingInput.JumpS,
                        [Terminal.TK_KP_2] =        TargettingInput.JumpS,
                        [Terminal.TK_J] =           TargettingInput.JumpS,
                        [Terminal.TK_UP] =          TargettingInput.JumpN,
                        [Terminal.TK_KP_8] =        TargettingInput.JumpN,
                        [Terminal.TK_K] =           TargettingInput.JumpN,
                        [Terminal.TK_RIGHT] =       TargettingInput.JumpE,
                        [Terminal.TK_KP_6] =        TargettingInput.JumpE,
                        [Terminal.TK_L] =           TargettingInput.JumpE,
                        [Terminal.TK_KP_7] =        TargettingInput.JumpNW,
                        [Terminal.TK_Y] =           TargettingInput.JumpNW,
                        [Terminal.TK_KP_9] =        TargettingInput.JumpNE,
                        [Terminal.TK_U] =           TargettingInput.JumpNE,
                        [Terminal.TK_KP_1] =        TargettingInput.JumpSW,
                        [Terminal.TK_B] =           TargettingInput.JumpSW,
                        [Terminal.TK_KP_3] =        TargettingInput.JumpSE,
                        [Terminal.TK_N] =           TargettingInput.JumpSE
                    }
                },
                InventoryMap = new KeyMap.StateMap<InventoryInput>()
                {
                    None = new Dictionary<int, InventoryInput>()
                    {
                        [Terminal.TK_SPACE]     = InventoryInput.Open,
                        [Terminal.TK_ENTER]     = InventoryInput.Open,
                        [Terminal.TK_KP_ENTER]  = InventoryInput.Open,
                        [Terminal.TK_RIGHT]     = InventoryInput.Open,
                        [Terminal.TK_UP]        = InventoryInput.MoveUp,
                        [Terminal.TK_KP_8]      = InventoryInput.MoveUp,
                        [Terminal.TK_DOWN]      = InventoryInput.MoveDown,
                        [Terminal.TK_KP_2]      = InventoryInput.MoveDown
                    }
                }
            };
        }
    }
}
