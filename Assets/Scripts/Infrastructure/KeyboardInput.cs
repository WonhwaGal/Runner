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
        public void RegisterInput()
        {
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

        [Obsolete]
        public Vector3 GetXAxisValue() => new Vector3(Input.GetAxis("Horizontal"), 0, 0);
    }
}