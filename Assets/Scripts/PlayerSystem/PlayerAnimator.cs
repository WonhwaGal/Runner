using UnityEngine;

public class PlayerAnimator
{
    private Animator _animator;
    private float _startAnimationSpeed;
    private static readonly int _speedAnimatorParameter = Animator.StringToHash("MoveSpeed");
    private static readonly int _fallTrigger = Animator.StringToHash("FallDown");
    public PlayerAnimator(Animator animator)
    {
        _animator = animator;
        _startAnimationSpeed = _animator.speed;
    }

    public void Move(float input)
    {
        _animator.SetFloat(_speedAnimatorParameter, input);
    }
    public void FallDown() => _animator.SetTrigger(_fallTrigger);
    public void FreezeAnimation() => _animator.speed = 0;
    public void ResumeAnimation() => _animator.speed = _startAnimationSpeed;
}
