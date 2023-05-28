using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        event Action<bool> OnPauseGame;
        event Action OnJump;
        event Action<float> OnChangingXValue;
        void RegisterInput();
        Vector3 GetXAxisValue();
    }
}