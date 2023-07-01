using UnityEngine;
using System;


namespace Infrastructure
{
    internal class KeyboardInput : IInput
    {
        public event Action<bool> OnPauseGame;
        public event Action OnJump;
        public event Action<float> OnChangingXValue;

        private bool _isPaused = false;
        private bool _isIgnored = false;
        public void RegisterInput()
        {
            if (!_isIgnored)
                GetXValue();

            GetYValue();
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _isPaused = !_isPaused;
                OnPauseGame?.Invoke(_isPaused);
            }
        }
        public void GetXValue()
        {
            if (Input.GetKey(KeyCode.LeftArrow))
                OnChangingXValue?.Invoke(-1);
            else if (Input.GetKey(KeyCode.RightArrow))
                OnChangingXValue?.Invoke(1);
        }

        public void GetYValue()
        {
            if (Input.GetAxis("Vertical") > 0)
                OnJump?.Invoke();
        }

        public void IgnoreInput(bool shouldBeIgnored)
        {
            _isIgnored = shouldBeIgnored;
            if (_isIgnored)
                UnityEngine.Debug.Log("input is ignored");
        }


        [Obsolete]
        public Vector3 GetXAxisValue() => new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }
}