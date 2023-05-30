using Infrastructure;
using UnityEngine;

namespace PlayerSystem
{
    [RequireComponent(typeof(Rigidbody), typeof(PlayerTriggerModule), typeof(BaseMover))]
    internal class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidBody;
        [SerializeField] private BaseMover _mover;
        [SerializeField] private PlayerTriggerModule _trigger;
        [SerializeField] private BoxCollider _collider;
        [SerializeField] private Animator _animator;
        private PlayerAnimator _playerAnimator;
        public PlayerAnimator PlayerAnimator { get => _playerAnimator;}

        public void Initialize(IInput input, float jumpForce, TriggerHandler handler)
        {
            _mover.Init(input, jumpForce);
            _trigger.Init(_collider, handler);
            _playerAnimator = new PlayerAnimator(_animator);
            _mover.OnChangingSpeed += _playerAnimator.Move;
            _mover.OnJumping += _playerAnimator.Jump;
        }

        public void ResumePlayerMove()
        {
            _mover.StartMove();
            PlayerAnimator.ResumeAnimation();
        }
        public void StopPlayerMove() => _mover.StopMoving();
        public void PausePlayerMove()
        {
            _mover.PauseMoving();
            PlayerAnimator.FreezeAnimation();
        }
        

        private void OnDestroy()
        {
            _mover.OnChangingSpeed -= _playerAnimator.Move;
            _mover.OnJumping -= _playerAnimator.Jump;
            _mover.Dispose();
        }

    }
}