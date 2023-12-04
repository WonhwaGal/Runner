using System;
using UnityEngine;

namespace GameUI
{
    internal interface IPauseView
    {
        GameObject Gameobject { get; }

        event Action OnBackToMenu;
        event Action OnExit;
    }
}