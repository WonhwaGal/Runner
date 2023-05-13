using Infrastructure;
using Tools;
using UnityEngine;

namespace PlayerSystem
{
    internal class PlayerMover : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IInput _input;
        private Vector3 _xInputVector;
        private Vector3 _movementVector = Vector3.zero;

        private float _speedMultiplier = Constants.gameMultiplier;
        private const float _xSpeedCorrector = 2.0f;
        private Transform _transform;

        private bool _canMove = false;

        public float SpeedMultiplier { get => _speedMultiplier; set => _speedMultiplier = value; }

        public void AssignInput(IInput input)
        {
            _input = input;
            _transform = gameObject.transform;
            _canMove = true;
        }

        private void Update()
        {

            if (!_canMove)
                return;

            _xInputVector = _xSpeedCorrector * _speedMultiplier * _input.GetXAxisValue();

            if (_transform.position.x < Constants.leftBorder && _xInputVector.x < 0 ||
                _transform.position.x > Constants.rightBorder && _xInputVector.x > 0)
                _xInputVector = Vector3.zero;

            _movementVector = Vector3.forward * _speedMultiplier + _xInputVector;
        }

        private void FixedUpdate()
        {
            if (!_canMove)
                return;

            _rigidbody.velocity = _movementVector;        // should i go to fixed update for that
        }
    }
}