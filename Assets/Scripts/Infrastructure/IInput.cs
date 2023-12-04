using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        event Action OnJump;
        event Action<float> OnChangingXValue;

        void RegisterInput();

        void IgnoreInput(bool shouldBeIgnored);
    }
}