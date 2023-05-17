using UnityEngine;
using DG.Tweening;
using Infrastructure;
using PlayerSystem;
using Tools;

internal class PlayerDOMover : BaseMover
{
    private IInput _input;
    private Transform _transform;
    private float _forwardMove = 1.5f;
    private float _forwardSpeed = 1.0f;
    private float _multiplier;

    private float _sideShift = 5.0f;
    private float _sideSpeed = 0.5f;
    private bool _isSideMoving;

    private float _jumpForce = 3;
    private bool _isJumping;

    private int _moveSides;

    public override void Init(IInput input, bool canJump)
    {
        _transform = transform;
        _input = input;
        _input.OnChangingXValue += ShiftToSides;
        if (canJump)
            _input.OnJump += Jump;

        _moveSides = 0;
        _multiplier = Constants.gameMultiplier;

        StartMove();
    }

    private void StartMove()
    {
        _transform.DOMoveZ(_transform.position.z + (_forwardMove * _multiplier), _forwardSpeed)
            .SetLoops(-1, LoopType.Incremental)
            .SetEase(Ease.Linear);
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
            _transform.DOJump(_transform.position + new Vector3(0, 0, _forwardMove) * _multiplier, _jumpForce, 1, _forwardSpeed)
                .SetEase(Ease.Linear)
                .OnComplete(() => _isJumping = false);
        }
    }
}
