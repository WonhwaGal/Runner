using UnityEngine;
using DG.Tweening;
using Infrastructure;
using PlayerSystem;
using Tools;
using System;

internal class PlayerDOMover : BaseMover
{
    private IInput _input;
    private Transform _transform;

    private Sequence _moveSequence;
    private float _forwardMove = 12.0f;
    private float _forwardSpeed = Constants.increaseSpeedSpan;

    private Tween _sideTween;
    private float _sideShift = 5.0f;
    private float _sideSpeed = 0.3f;
    private bool _isSideMoving;

    private Tween _jumpTween;
    private float _jumpForce;
    private bool _isJumping;
    private bool _canJump;

    private int _moveSides;
    private float _moveDirection;
    private float _fallBack = 2.5f;

    public override void Init(IInput input, float jumpForce)
    {
        _transform = transform;
        _input = input;
        _isSideMoving = true;
        SetDefaultLane(0);
        SetJumpConditions(jumpForce);

        _input.OnChangingXValue += ShiftToSides;
    }

    private void SetJumpConditions(float jumpForce)
    {
        _jumpForce = jumpForce;
        if (jumpForce == 0)
            _canJump = false;
        else
        {
            _canJump = true;
            _input.OnJump += Jump;
        }
    }

    public void TurnAround() => _transform.DOLocalRotate(new Vector3(0, 360, 0), 1.0f);

    public override void SetDefaultLane(int number)
    {
        _moveSides = number;
        OnChangingLane?.Invoke(_moveSides);
    } 

    public override void StartMove()
    {
        if (_moveSequence != null)
        {
            _moveSequence.Play();
            _isJumping = false;
            return;
        }

        OnStartRunning?.Invoke();
        _isSideMoving = false;

        _moveSequence = DOTween.Sequence();
        _moveSequence.Append(_transform.DOMoveZ(_transform.position.z + _forwardMove, _forwardSpeed)
            .SetEase(Ease.Linear))
            .AppendCallback(IncreaseSpeed)
            .SetLoops(Int32.MaxValue, LoopType.Incremental);
    }

    private void IncreaseSpeed()
    {
        _moveSequence.timeScale += 0.01f;
        OnChangingSpeed?.Invoke(_forwardSpeed * _moveSequence.timeScale);
        OnSpeedingUp?.Invoke(_moveSequence.timeScale);
    }

    private void ShiftToSides(float xValue)
    {
        if (_isJumping || _isSideMoving)
            return;

        _moveDirection = xValue;
        if (xValue > 0 && _moveSides < 1)
            MoveToSide(1);
        else if (xValue < 0 && _moveSides > -1)
            MoveToSide(-1);
    }

    private void MoveToSide(int number)
    {
        _isSideMoving = true;
        _moveSides += number;
        OnChangingLane?.Invoke(_moveSides);
        _sideTween = _transform.DOMoveX(_transform.position.x + _sideShift * number, _sideSpeed)
            .OnComplete(() => _isSideMoving = false);

        if (_moveSequence != null)
            _sideTween.timeScale = _moveSequence.timeScale;
    }

    private void Jump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            OnJumping?.Invoke();
            _jumpTween = _transform
                .DOJump(_transform.position + new Vector3(0, 0, _forwardMove), _jumpForce, 1, _forwardSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isJumping = false);
            _jumpTween.timeScale = _moveSequence.timeScale;
        }
    }

    public override void PauseMoving()
    {
        _moveSequence.Pause();
        _isJumping = true;
    }

    public override void StopMoving()
    {
        _moveSequence.Kill();
        _isJumping = true;

        if (!_isSideMoving)
            transform.DOMoveZ(_transform.position.z - _fallBack, _forwardSpeed);
        else
            transform.DOMoveX(_transform.position.x + (_fallBack * _moveDirection * -1), _forwardSpeed);
    }

    public override void Dispose()
    {
        _input.OnChangingXValue -= ShiftToSides;
        if (_canJump)
            _input.OnJump -= Jump;
    }
}
