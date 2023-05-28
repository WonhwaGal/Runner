using Infrastructure;
using System;
using UnityEngine;

namespace PlayerSystem
{
    internal abstract class BaseMover: MonoBehaviour
    {
        public Action<float> OnChangingSpeed;
        public abstract void Init(IInput input, bool canJump);
        public abstract void StartMove();
        public abstract void PauseMoving();
        public abstract void StopMoving();
        public abstract void Dispose();
    }
}