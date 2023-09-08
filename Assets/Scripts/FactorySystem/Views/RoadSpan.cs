using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System;
using System.Collections;

namespace Factories
{
    internal class RoadSpan : MonoBehaviour, IRespawnable, IRoadSpan
    {
        [SerializeField] private GameObject _obstacleSet_1;
        [SerializeField] private GameObject _obstacleSet_2;
        [SerializeField] private List<Transform> _coinSpots_1;
        [SerializeField] private List<Transform> _coinSpots_2;
        [SerializeField] private List<Transform> _upgradeSpots_1;
        [SerializeField] private List<Transform> _upgradeSpots_2;
        [SerializeField] private RoadSpanType _roadType;

        public Action<RoadSpanType> OnTurnedOff;
        public Action<bool> OnTurning;

        private bool _isActive;
        private int _setNumber;

        private int _playerLane;
        private Transform _rootObject;
        private Sequence _turnSequence;
        private List<Transform> _childRoadSpans;

        public List<IRespawnable> Collectables { get; private set; }
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;
        public RoadSpanType RoadType { get => _roadType; }
        public int PlayerLane { get => _playerLane; set => _playerLane = value; }
        public Transform RootObject { get => _rootObject; set => _rootObject = value; }
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

        public void Activate()
        {
            _childRoadSpans = new List<Transform>();
            Collectables = new List<IRespawnable>();
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

        public RoadSpan ReturnLeftChild() => _childRoadSpans[0].GetComponent<RoadSpan>();

        public void MakeTurn(float shift, int yRotation, float turnTimescale)
        {
            Debug.Log("timescale is " + turnTimescale);
            _turnSequence = DOTween.Sequence();
            _turnSequence.timeScale = turnTimescale;
            _turnSequence.Append(transform.DOMoveX(transform.position.x + shift, 1.0f))
                        .Join(transform.DORotate(new Vector3(0, yRotation, 0), 1.0f))
                        .OnComplete(KillSequence);
        }

        private void KillSequence() => _turnSequence.Kill();

        public void AcceptChildRespawnable(IRespawnable childObject, RespawnableType type)
        {
            childObject.BodyObject.transform.SetParent(transform, true);
            if (type == RespawnableType.Road)
                _childRoadSpans.Add(childObject.BodyObject.transform);

            if (type == RespawnableType.Coin || type == RespawnableType.Upgrade)
            {
                Collectables.Add(childObject);
            }
        }

        public void UnparentChildObjects()
        {
            Debug.Log($"{gameObject.name} went FULL UNparent");
            for (int i = 0; i < _childRoadSpans.Count; i++)
                _childRoadSpans[i].SetParent(_rootObject);

            for(int i = 0; i < Collectables.Count; i++)
                Collectables[i].Deactivate();

            _childRoadSpans.Clear(); 
            Collectables.Clear();
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
            OnTurnedOff?.Invoke(RoadType);
        }

        public void Deactivate()
        {
            Debug.Log(gameObject.name + "deactivated");
            StopCoroutine(CheckForDeactivate());
            UnparentChildObjects();
            _isActive = false;
            transform.forward = Vector3.forward;
            transform.rotation = Quaternion.identity;
            transform.SetParent(_rootObject);
            gameObject.SetActive(false);
        }
    }
}