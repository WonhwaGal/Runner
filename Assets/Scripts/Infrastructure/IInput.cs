using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        event Action OnPauseGame;
        event Action OnJump;
        event Action<float> OnChangingXValue;
        void RegisterInput();
        void GetXValue();
        void GetYValue();
        Vector3 GetXAxisValue();
    }
}