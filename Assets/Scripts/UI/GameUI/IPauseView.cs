﻿using System;
using UnityEngine;

namespace GameUI
{
    internal interface IPauseView
    {
        event Action<bool> OnContinueGame;
        event Action OnBackToMenu;
        event Action OnExit;

        GameObject Gameobject { get; }
    }
}