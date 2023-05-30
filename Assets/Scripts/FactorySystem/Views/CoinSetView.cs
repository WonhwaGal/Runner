using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class CoinSetView : MonoBehaviour, IRespawnable
    {
        [SerializeField] private List<CollectableObject> _collectables;
        private bool _isActive;

        public List<CollectableObject> Collectables => _collectables;
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;
        private void OnBecameInvisible() => Deactivate();
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
            _isActive = false;
            gameObject.SetActive(false);
        }

        public void PauseChild(bool isPaused)
        {
            for (int i = 0; i < _collectables.Count; i++)
                _collectables[i].PauseAnimation(isPaused);
        }
    }
}