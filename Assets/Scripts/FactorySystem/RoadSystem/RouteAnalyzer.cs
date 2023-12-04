using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class RouteAnalyzer : IRouteAnalyzer
    {
        private readonly IPool<RoadSpan> _roadPool;
        private readonly List<RoadSpan> _roadSpans;

        private Vector3 _currentShift;
        private Vector3 _forwardShift = new (0, 0, 100);
        private Vector3 _leftShift = new (-100, 0, 0);
        private Vector3 _rightShift = new (100, 0, 0);

        private RoadSpan _startSpan;
        private readonly List<RoadSpan> _endSpans;
        private int _currentLane;
        private int _roadSpansCreated = 0;
        private const int MaxRoadSpans = 8;

        private bool _hasTurns = false;
        private float _turnTimescale = 1;

        public RouteAnalyzer(Transform firstRoadSpan, IPool<RoadSpan> roadFactory)
        {
            _roadPool = roadFactory;
            _currentShift = _forwardShift;
            _roadSpans = new List<RoadSpan>();
            _endSpans = new List<RoadSpan>();

            RegisterFirstRoadSpan(firstRoadSpan);
            for (int i = 0; i < _roadPool.InactiveObjects.Count; i++)
                RegisterRoadSpan(_roadPool.InactiveObjects[i]);
        }

        public event Action<IRoadSpan> RequestForCoins;
        public event Action<IRoadSpan> RequestForUpgrades;
        public event Action<bool> OnLaneChangingBlocked;

        private void RegisterFirstRoadSpan(Transform firstRoadSpan)
        {
            _roadSpansCreated++;
            _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
            _startSpan.Activate();
            _startSpan.OnTurnedOff += OnTurnOffRoad;
            _startSpan.RootObject = _roadPool.RootObject;
            _endSpans.Add(_startSpan);
        }

        public void OnTurnOffRoad(RoadSpan road)
        {
            if (road.HasTurns)
                _hasTurns = false;

            _roadPool.Despawn(road);
            PlanRoadAhead();
        }

        public void PlanRoadAhead()
        {
            int currentEndsNumber = _endSpans.Count;

            if (_roadSpansCreated >= MaxRoadSpans)
                return;

            _roadSpansCreated--;
            for (int i = 0; i < currentEndsNumber; i++)
            {
                _currentShift = _endSpans[i].RoadType switch
                {
                    RoadSpanType.RightTurn => _rightShift,
                    RoadSpanType.LeftTurn => _leftShift,
                    RoadSpanType.TwoWays => Vector3.zero,
                    _ => _forwardShift
                };

                PlaceRoadSpan(_endSpans[i], _currentShift);
            }
        }

        private void PlaceRoadSpan(RoadSpan previousSpan, Vector3 currentShift)
        {
            _currentShift = currentShift;
            var roadSpan = SpawnRoadSpan();
            if(!_hasTurns && roadSpan.HasTurns)
                _hasTurns = true;

            if (currentShift == Vector3.zero)
            {
                var roadSpan2 = SpawnRoadSpan();
                UpdateRoadSpanPosition(roadSpan2, previousSpan, _leftShift);
                _currentShift = _rightShift;
                _endSpans.Add(roadSpan2);
            }
            UpdateRoadSpanPosition(roadSpan, previousSpan, _currentShift);

            _endSpans[_endSpans.IndexOf(previousSpan)] = roadSpan;
        }

        private RoadSpan SpawnRoadSpan(bool shouldSaveSpawn = false)
        {
            RoadSpan roadSpan;

            if (shouldSaveSpawn)
                roadSpan = _roadPool.Spawn(true);
            else
                roadSpan = _roadPool.Spawn();

            if (_hasTurns && roadSpan.HasTurns)
            {
                _roadPool.Despawn(roadSpan);
                return SpawnRoadSpan(true);
            }

            if (!_roadSpans.Contains(roadSpan))
                RegisterRoadSpan(roadSpan);
            _roadSpansCreated++;

            return roadSpan;
        }

        private void UpdateRoadSpanPosition(RoadSpan currentSpan, RoadSpan previousSpan, Vector3 currentShift)
        {
            if (currentShift == _rightShift)
                PlaceAsChildObject(currentSpan, previousSpan, 1, _rightShift);
            else if (currentShift == _leftShift)
                PlaceAsChildObject(currentSpan, previousSpan, -1, _leftShift);
            else if (currentShift == _forwardShift)
                PlaceAsChildObject(currentSpan, previousSpan, 0, _forwardShift);

            RequestForCoins?.Invoke(currentSpan);
            RequestForUpgrades?.Invoke(currentSpan);
        }

        private void PlaceAsChildObject(RoadSpan currentSpan, RoadSpan previousSpan, int direction, Vector3 currentShift)
        {
            currentSpan.transform.position = previousSpan.transform.position;
            currentSpan.transform.rotation = previousSpan.transform.rotation;

            currentSpan.transform.rotation *= Quaternion.Euler(0, 90 * direction, 0);
            previousSpan.AcceptChildRespawnable(currentSpan, RespawnableType.Road);
            currentSpan.transform.localPosition += currentShift;
        }

        public void RegisterRoadSpan(RoadSpan roadSpan)
        {
            roadSpan.OnTurning += BlockLaneChanging;
            roadSpan.OnTurnedOff += OnTurnOffRoad;
            roadSpan.RootObject = _roadPool.RootObject;
            _roadSpans.Add(roadSpan);
        }

        public void UpdatePlayerLane(int number)
        {
            _currentLane = number;
            for (int i = 0; i < _roadSpans.Count; i++)
                _roadSpans[i].PlayerLane = number;
        }

        private void BlockLaneChanging(bool shouldBlock) => OnLaneChangingBlocked?.Invoke(shouldBlock);

        public void SetTurnTimescale(float timeScale) => _turnTimescale = timeScale;

        public void CheckForTurn(RoadSpan road)
        {
            if (road.RoadType == RoadSpanType.Straight || _currentLane == 0)
                return;

            BlockLaneChanging(true);
            if (_currentLane == 1 && (road.RoadType == RoadSpanType.TwoWays || road.RoadType == RoadSpanType.RightTurn))
            {
                road.MakeTurn(4.9f, -90, _turnTimescale);
                UpdatePreviousSpan(road.RoadType, _currentLane);
                return;
            }

            if (_currentLane == -1 && (road.RoadType == RoadSpanType.TwoWays || road.RoadType == RoadSpanType.LeftTurn))
            {
                road.MakeTurn(-4.9f, 90, _turnTimescale);
                UpdatePreviousSpan(road.RoadType, _currentLane);
            }
        }

        private void UpdatePreviousSpan(RoadSpanType roadType, int chosenLane)
        {
            if (roadType != RoadSpanType.TwoWays)
                return; 

            if (chosenLane == -1)
                _endSpans.Remove(_endSpans[0]);
            else if (chosenLane == 1)
                _endSpans.Remove(_endSpans[1]);
        }

        public void Dispose()
        {
            _startSpan.OnTurnedOff -= OnTurnOffRoad;
            for (int i = 0; i < _roadSpans.Count; i++)
            {
                _roadSpans[i].OnTurnedOff -= OnTurnOffRoad;
                _roadSpans[i].OnTurning -= BlockLaneChanging;
            }
        }
    }
}