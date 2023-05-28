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
    private float _forwardSpeed = 1.0f;

    private float _sideShift = 5.0f;
    private float _sideSpeed = 0.5f;
    private bool _isSideMoving;

    private float _jumpForce = 4;
    private bool _isJumping;
    private bool _canJump;

    private int _moveSides;

    public override void Init(IInput input, bool canJump)
    {
        _transform = transform;
        _input = input;
        _canJump = canJump;
        _input.OnChangingXValue += ShiftToSides;
        if (_canJump)
            _input.OnJump += Jump;

        _moveSides = 0;
        StartMove();
    }

    public override void StartMove()
    {
        if (_moveSequence != null)
        {
            _moveSequence.Play();
            return;
        }
 
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
    }

    private void ShiftToSides(float xValue)
    {
        if (_isJumping || _isSideMoving)
            return;

        if (xValue > 0 && _moveSides < 1)
            MoveToSide(1);
        else if (xValue < 0 && _moveSides > -1)
            MoveToSide(-1);
    }

    private void MoveToSide(int number)
    {
        _isSideMoving = true;
        _transform.DOMoveX(_transform.position.x + _sideShift * number, _sideSpeed)
            .OnComplete(() =>
            { 
                 _moveSides += number;
                 _isSideMoving = false;
            });
    }

    private void Jump()
    {
        if (!_isJumping)
        {
            _isJumping = true;
            _transform.DOJump(_transform.position + new Vector3(0, 0, _forwardMove), _jumpForce, (int)_forwardSpeed, _forwardSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isJumping = false);
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
        transform.DOMoveZ(_transform.position.z - 1.2f, _forwardSpeed);
        _isJumping = true;
    }

    public override void Dispose()
    {
        _input.OnChangingXValue -= ShiftToSides;
        if (_canJump)
            _input.OnJump -= Jump;
    }
}
