using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        event Action OnJump;
        event Action<float> OnChangingXValue;
        void RegisterInput();
        Vector3 GetXAxisValue();

        void IgnoreInput(bool shouldBeIgnored);
    }
}