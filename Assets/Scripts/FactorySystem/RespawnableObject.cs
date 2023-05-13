using UnityEngine;

namespace Factories
{
    internal class RespawnableObject : MonoBehaviour
    {
        private bool isActive;
        public GameObject BodyObject { get; private set; }
        public bool IsActive { get => isActive; private set => isActive = value; }
        private void OnEnable() => BodyObject = gameObject;

        private void OnBecameVisible() => Activate();   // Questionable!

        private void OnBecameInvisible() => Deactivate();

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            gameObject.SetActive(false);
            IsActive = false;
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}