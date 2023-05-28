using UnityEngine;
using System.Collections.Generic;
using Collectables;

namespace Factories
{
    internal class RoadSpan : MonoBehaviour, IRespawnable
    {
        [SerializeField] private List<CollectableObject> _collectables;
        [SerializeField] private List<Transform> _coinSpots;
        [SerializeField] private List<Transform> _upgradeSpots;
        private bool _isActive;

        public List<Transform> Spots { get => _coinSpots; }
        public List<Transform> UpgradeSpots { get => _upgradeSpots; }
        public List<CollectableObject> Collectables => _collectables;
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;
        private void OnBecameInvisible() => Deactivate();
        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);

            if (_collectables.Count > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }

        public void PauseChild(bool isPaused) { }
    }
}