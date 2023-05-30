using Infrastructure;
using UnityEngine;
using Tools;

namespace PlayerSystem
{
    internal class PlayerRBJumper : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private IInput _input;

        private Vector3 _jumpVector = Constants.jumpVector;
        private float _jumpSpan = 0.1f;
        private const float _jumpMultiplier = 16;

        public bool IsJumping { get; private set; }

        public void Init(IInput input, Rigidbody rigidbody)
        {
            _input = input;
            _rigidbody = rigidbody;
            _input.OnJump += StartJump;
        }

        private void Update()
        {
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
            //_jumpSpan = Constants.jumpSpan;
        }
    }
}