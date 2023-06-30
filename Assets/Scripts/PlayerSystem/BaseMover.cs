using Infrastructure;
using System;
using UnityEngine;

namespace PlayerSystem
{
    internal abstract class BaseMover: MonoBehaviour
    {
        public Action OnJumping;
        public Action<float> OnChangingSpeed;
        public Action OnStartRunning;
        public Action<float> OnSpeedingUp;
        public Action<int> OnChangingLane;

        public abstract void Init(IInput input, float jumpForce);
        public abstract void SetLane(int number);
        public abstract void StartMove();
        public abstract void PauseMoving();
        public abstract void StopMoving();
        public abstract void Dispose();
    }
}