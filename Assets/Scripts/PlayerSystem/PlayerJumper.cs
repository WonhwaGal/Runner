using Infrastructure;
using UnityEngine;
using Tools;

namespace PlayerSystem
{
    internal class PlayerJumper : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rigidbody;

        private IInput _input;
        private bool _canJump;

        private bool _isJumping = false;
        private Vector3 _jumpVector = Constants.jumpVector;
        private float _jumpSpan = 0.1f;
        private const float _jumpMultiplier = 16;

        public bool IsJumping { get => _isJumping; set => _isJumping = value; }

        public void AssignInput(IInput input)
        {
            _input = input;
            _canJump = true;
        }

        private void Update()
        {
            if (_canJump && _input.GetYAxisValue() > 0)
                StartJump();

            if (IsJumping && _jumpSpan > 0)
                _jumpSpan -= Time.deltaTime;
        }

        private void FixedUpdate()
        {
            if (IsJumping && _jumpSpan > 0)
                _rigidbody.velocity = _jumpVector;
            if (IsJumping)
                _rigidbody.AddForce(Physics.gravity * _jumpMultiplier);
        }

        private void StartJump() => IsJumping = true;
        public void FinishJump()
        {
            IsJumping = false;
            _jumpSpan = Constants.jumpSpan;
        }
    }
}