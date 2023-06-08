using UnityEngine;
using System.Collections.Generic;
using Collectables;

namespace Factories
{
    internal class RoadSpan : MonoBehaviour, IRespawnable
    {
        [SerializeField] private List<CollectableObject> _collectables;
        [SerializeField] private GameObject _obstacleSet_1;
        [SerializeField] private GameObject _obstacleSet_2;
        [SerializeField] private List<Transform> _coinSpots_1;
        [SerializeField] private List<Transform> _coinSpots_2;
        [SerializeField] private List<Transform> _upgradeSpots_1;
        [SerializeField] private List<Transform> _upgradeSpots_2;
        private bool _isActive;
        private int _setNumber;

        public List<Transform> CoinSpots
        {
            get
            {
                if (_setNumber == 1)
                    return _coinSpots_1;
                else
                    return _coinSpots_2;
            }
        }
        public List<Transform> UpgradeSpots
        {
            get
            {
                if (_setNumber == 1)
                    return _upgradeSpots_1;
                else
                    return _upgradeSpots_2;
            }
        }

        public List<CollectableObject> Collectables => _collectables;
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;

        private void OnBecameInvisible() => Deactivate();

        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
            SetUpScene();
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.SetActive(false);

            if (_collectables.Count > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        private void SetUpScene()
        {
            _setNumber = Random.Range(0, 2) % 2 == 0 ? 1 : 2;
            if (_setNumber == 1)
            {
                _obstacleSet_1.SetActive(true);
                _obstacleSet_2.SetActive(false);
            }
            else
            {
                _obstacleSet_2.SetActive(true);
                _obstacleSet_1.SetActive(false);
            }
        }
    }
}