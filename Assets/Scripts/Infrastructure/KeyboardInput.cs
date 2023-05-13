using UnityEngine;

namespace Infrastructure
{
    internal class KeyboardInput: IInput
    {
        public Vector3 GetXAxisValue() => new Vector3(Input.GetAxis("Horizontal"), 0, 0);

        public float GetYAxisValue() => Input.GetAxis("Vertical");
    }
}