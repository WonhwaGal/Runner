using UnityEngine;

namespace PlayerSystem
{
    public sealed class PlayerAnimator
    {
        private readonly Animator _animator;
        private readonly float _startAnimationSpeed;
        private static readonly int _speedAnimatorParameter = Animator.StringToHash("MoveSpeed");
        private static readonly int _fallTrigger = Animator.StringToHash("FallDown");
        private static readonly int _jumpTrigger = Animator.StringToHash("JumpTrigger");
        private static readonly int _hitInJumpTrigger = Animator.StringToHash("HitInJump");

        public PlayerAnimator(Animator animator)
        {
            _animator = animator;
            _startAnimationSpeed = _animator.speed;
        }

        public void Move(float input) => _animator.SetFloat(_speedAnimatorParameter, input);

        public void Jump() => _animator.SetTrigger(_jumpTrigger);

        public void FallDown()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("Jumping"))
                _animator.SetTrigger(_hitInJumpTrigger);
            else
                _animator.SetTrigger(_fallTrigger);
        }

        public void FreezeAnimation() => _animator.speed = 0;

        public void ResumeAnimation() => _animator.speed = _startAnimationSpeed;
    }
}