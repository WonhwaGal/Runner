using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;
using DG.Tweening;

namespace Factories
{
    internal class RoadSystem
    {
        private GenericFactory<RoadSpan> _roadFactory;
        private Vector3 _firstPos;
        private const float _roadCreateSpeed = 7;
        private float _increaseSpeed = Constants.increaseSpeedSpan;
        private Vector3 _shift = new Vector3(0, 0, 100);

        Sequence _roadSequence;
        Sequence _speedSequence;

        public event Action<List<Transform>> OnRoadForCoins;
        public event Action<List<Transform>> OnRoadForUpdates;

        public RoadSystem(Transform firstRoadSpan)
        {
            _firstPos = firstRoadSpan.position;
            CreateRoadFactory();
        }

        private void CreateRoadFactory()
        {
            _roadFactory = new SingleFactory<RoadSpan>("RoadPrefabs");
            _roadFactory.AddPrefabNameToList("RoadBlock1");
            _roadFactory.CreateListOfObjects();
        }

        public void StartRoadSpawn()
        {
            SetRoadRespawn();
            IncreaseRespawn();
        }
        public void PauseRoadSpawn()
        {
            _roadSequence.Pause();
            _speedSequence.Pause();
        }
        public void StopRoadSpawn()
        {
            _roadSequence.Kill();
            _speedSequence.Kill();
        }

        private void SetRoadRespawn()
        {
            if (_roadSequence != null)
            {
                _roadSequence.Play();
                return;
            }
            _roadSequence = DOTween.Sequence();
            _roadSequence.AppendCallback(PutRoadAhead)
                .AppendInterval(_roadCreateSpeed)
                .SetLoops(-1);
        }

        private void IncreaseRespawn()
        {
            _speedSequence = DOTween.Sequence();
            _speedSequence.AppendInterval(_increaseSpeed)
                .AppendCallback(IncreaseSpeed)
                .SetLoops(Int32.MaxValue);
        }
        private void IncreaseSpeed() => _roadSequence.timeScale += 0.01f;

        private void PutRoadAhead()
        {
            var roadSpan = _roadFactory.Spawn();
            roadSpan.transform.position = _firstPos + _shift;
            _firstPos += _shift;

            OnRoadForCoins?.Invoke(roadSpan.Spots);
            OnRoadForUpdates?.Invoke(roadSpan.UpgradeSpots);
        }
    }
}