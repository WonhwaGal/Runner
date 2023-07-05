using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class CoinSetView : MonoBehaviour, IRespawnable
    {
        [SerializeField] private List<CollectableObject> _collectables;
        private bool _isActive;

        public Transform RootObject { get; set; }
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;

        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
            if (_collectables.Count > 0)
            {
                for (int i = 0; i < _collectables.Count; i++)
                    _collectables[i].gameObject.SetActive(true);
            }
        }

        public void Deactivate()
        {
            Debug.Log("deactivated coin set");
            _isActive = false;
            gameObject.transform.SetParent(RootObject);
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            if (_collectables.Count > 0)
            {
                for (int i = 0; i < _collectables.Count; i++)
                    _collectables[i].ReturnToPlace();
            }
        }

        public void PauseChild(bool isPaused)
        {
            for (int i = 0; i < _collectables.Count; i++)
                _collectables[i].PauseAnimation(isPaused);
        }
    }
}