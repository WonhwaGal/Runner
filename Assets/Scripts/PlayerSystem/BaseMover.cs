using Infrastructure;
using UnityEngine;

namespace PlayerSystem
{
    internal abstract class BaseMover: MonoBehaviour
    {
        public abstract void Init(IInput input, bool canJump);
    }
}