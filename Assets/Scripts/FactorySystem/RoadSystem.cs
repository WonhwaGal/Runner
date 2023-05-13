using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Factories
{
    internal class RoadSystem
    {
        private List<Timer> _timers;
        private GenericFactory<RoadSpan> _roadFactory;
        private Vector3 _firstPos;
        private readonly float _roadCreateSpeed;
        private Vector3 _shift = new Vector3(0, 0, 100);

        public event Action<List<Transform>> OnBuildingRoadSpan;

        public RoadSystem(List<Timer> timers, Transform firstRoadSpan)
        {
            _timers = timers;
            _firstPos = firstRoadSpan.position;
            _roadCreateSpeed = 50 / Constants.gameMultiplier;
            CreateRoadFactory();
            SetRoadTimer();
        }

        private void CreateRoadFactory()
        {
            _roadFactory = new SingleFactory<RoadSpan>("RoadPrefabs");
            _roadFactory.AddPrefabNameToList("Road_0");
            _roadFactory.AddPrefabNameToList("Road_1");
            _roadFactory.AddPrefabNameToList("Road_2");
            _roadFactory.CreateListOfObjects();
        }

        private void SetRoadTimer()
        {
            Timer _roadTimer = new Timer(PutRoadAhead, _roadCreateSpeed, true, true);
            _timers.Add(_roadTimer);
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