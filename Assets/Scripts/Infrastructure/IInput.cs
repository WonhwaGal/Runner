using UnityEngine;
using System;

namespace Infrastructure
{
    internal interface IInput
    {
        Vector3 GetXAxisValue();
        float GetYAxisValue();
    }
}