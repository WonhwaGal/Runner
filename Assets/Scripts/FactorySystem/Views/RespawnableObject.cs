using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRespawnable
    {
        List<CollectableObject> Collectables { get; }
        public GameObject BodyObject { get;  }
        public bool IsActive { get; }
        void Activate();
        void Deactivate();
        void PauseChild(bool isPaused);
    }
    internal class RespawnableObject : MonoBehaviour
    {
        [SerializeField] private List<CollectableObject> _collectables;
        private bool _isActive;
        protected bool _hasChildObjects;

        public GameObject BodyObject { get; private set; }
        public bool IsActive { get => _isActive; private set => _isActive = value; }

        private void OnEnable() => BodyObject = gameObject;

        private void OnBecameInvisible() => Deactivate();

        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        private void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);

            if (_hasChildObjects)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
        public void Pause(bool isPaused)
        {
            for(int i = 0; i < _collectables.Count; i++)
                _collectables[i].PauseAnimation(isPaused);
        }
    }
}