using UnityEngine;

namespace Infrastructure
{
    internal class KeyboardInput: IInput
    {
        public Vector3 GetXAxisValue()
        {
            return new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }
    }
}