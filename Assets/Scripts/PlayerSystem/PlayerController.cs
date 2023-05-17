using Infrastructure;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerTriggerModule), typeof(BaseMover))]
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private BaseMover _mover;
        //[SerializeField] private PlayerRBJumper _jumper;
        [SerializeField] private PlayerTriggerModule _trigger;
        [SerializeField] private CapsuleCollider _collider;
        public bool CanJump { get; private set; }

        public void Initialize(IInput input, bool canJump, TriggerHandler handler)
        {
            CanJump = canJump;
            _mover.Init(input, canJump);
            _trigger.Init(_collider, handler);
        }

    }
}