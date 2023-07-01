using UnityEngine;
using System.Collections.Generic;
using Collectables;
using DG.Tweening;
using System;
using System.Collections;

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
        [SerializeField] private RoadSpanType _roadType;

        public Action OnTurnedOff;
        public Action<bool> OnTurning;
        public Action<RoadSpan, int> OnSettingNextRoadSpan;

        private bool _isActive;
        private int _setNumber;

        private int _playerLane;
        private Transform _factoryParentTransfom;
        private Sequence _turnSequence;
        private List<Transform> _childRoadSpans;

        public List<CollectableObject> Collectables => _collectables;
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;
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

        public RoadSpanType RoadType { get => _roadType; }
        public int PlayerLane { get => _playerLane; set => _playerLane = value; }
        public Transform FactoryParentTransfom { get => _factoryParentTransfom; set => _factoryParentTransfom = value; }

        public void Activate()
        {
            _childRoadSpans = new List<Transform>();
            _isActive = true;
            gameObject.SetActive(true);
            SetUpScene();
        }

        private void SetUpScene()
        {
            _setNumber = UnityEngine.Random.Range(0, 2) % 2 == 0 ? 1 : 2;
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

        public void CheckForTurn()
        {
            if (_roadType == RoadSpanType.Straight || PlayerLane == 0)
                return;
            OnTurning?.Invoke(true);
            if (PlayerLane == 1 && (_roadType == RoadSpanType.TwoWays || _roadType == RoadSpanType.RightTurn))
            {
                Debug.Log("turning coz my lane is 1");
                MakeTurn(4.9f, -90);
                return;
            }

            if (PlayerLane == -1 && (_roadType == RoadSpanType.TwoWays || _roadType == RoadSpanType.LeftTurn))
            {
                MakeTurn(-4.9f, 90);
                RoadSpan nextRoadSpan = _childRoadSpans[0].GetComponent<RoadSpan>();
                OnSettingNextRoadSpan?.Invoke(nextRoadSpan, PlayerLane);
            }
        }

        private void MakeTurn(float shift, int yRotation)
        {
            _turnSequence = DOTween.Sequence();
            _turnSequence.Append(transform.DOMoveX(transform.position.x + shift, 1.0f))
                        .Join(transform.DORotate(new Vector3(0, yRotation, 0), 1.0f))
                        .OnComplete(KillSequence);
        }

        private void KillSequence() => _turnSequence.Kill();

        public void AcceptAChildObject(Transform span)
        {
            span.SetParent(transform, true);
            _childRoadSpans.Add(span);
        }

        public void UnparentChildObjects()
        {
            Debug.Log($"{gameObject.name} went UNparent");
            for (int i = 0; i < _childRoadSpans.Count; i++)
                _childRoadSpans[i].transform.SetParent(_factoryParentTransfom);

            _childRoadSpans.Clear(); ;
        }

        private void OnBecameInvisible()
        {
            if (gameObject.activeInHierarchy)
                StartCoroutine(CheckForDeactivate());
        }

        private IEnumerator CheckForDeactivate()
        {
            bool shouldDeactivate = false;
            while (!shouldDeactivate)
            {
                float saveDistance = Camera.main.transform.position.z - 70;
                if (transform.position.z > saveDistance)   // transform.rotation.y % 90 == 0
                    yield return new WaitForSeconds(1);
                else
                    shouldDeactivate = true;
            }

            Deactivate();
            OnTurnedOff?.Invoke();
        }

        public void Deactivate()
        {
            StopCoroutine(CheckForDeactivate());
            UnparentChildObjects();
            _isActive = false;
            transform.forward = Vector3.forward;
            transform.rotation = Quaternion.identity;
            transform.SetParent(_factoryParentTransfom);
            gameObject.SetActive(false);
        }
    }
}