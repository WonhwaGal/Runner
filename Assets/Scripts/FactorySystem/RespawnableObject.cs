using UnityEngine;

namespace Factories
{
    internal class RespawnableObject : MonoBehaviour
    {
        private bool _isActive;
        protected bool _hasChildObjects;
        public GameObject BodyObject { get; private set; }
        public bool IsActive { get => _isActive; private set => _isActive = value; }


        private void OnEnable() => BodyObject = gameObject;

        private void OnBecameInvisible()
        {
            gameObject.SetActive(false);
            IsActive = false;
        }

        public void Activate()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            
            //if (_hasChildObjects)
            //{
            //    for (int i = 0; i < transform.childCount; i++)
            //    {
            //        transform.GetChild(i).gameObject.SetActive(true);
            //    }
            //}
        }
    }
}