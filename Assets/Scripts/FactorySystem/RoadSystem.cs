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
        private readonly float _roadCreateSpeed;
        private Vector3 _shift = new Vector3(0, 0, 100);

        public event Action<List<Transform>> OnBuildingRoadSpan;

        public RoadSystem(Transform firstRoadSpan)
        {
            _firstPos = firstRoadSpan.position;
            _roadCreateSpeed = 50 / Constants.gameMultiplier;
            CreateRoadFactory();

            SetRoadRespawn();
        }

        private void CreateRoadFactory()
        {
            _roadFactory = new SingleFactory<RoadSpan>("RoadPrefabs");
            _roadFactory.AddPrefabNameToList("Road_0");
            _roadFactory.AddPrefabNameToList("Road_1");
            _roadFactory.AddPrefabNameToList("Road_2");
            _roadFactory.CreateListOfObjects();
        }

        private void SetRoadRespawn()
        {
            Sequence sequence = DOTween.Sequence();
            sequence.AppendInterval(_roadCreateSpeed).AppendCallback(PutRoadAhead).SetLoops(-1);
        }

        private void PutRoadAhead()
        {
            var roadSpan = _roadFactory.Spawn();
            roadSpan.transform.position = _firstPos + _shift;
            _firstPos += _shift;

            OnBuildingRoadSpan?.Invoke(roadSpan.Spots);
        }
    }
}