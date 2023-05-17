using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        event Action OnJump;
        event Action<float> OnChangingXValue;
        void GetXValue();
        void GetYValue();
        Vector3 GetXAxisValue();
    }
}