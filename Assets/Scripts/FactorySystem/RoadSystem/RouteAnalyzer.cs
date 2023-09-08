using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class RouteAnalyzer : IRouteAnalyzer
    {
        public event Action<IRoadSpan> RequestForCoins;
        public event Action<IRoadSpan> RequestForUpgrades;
        public event Action<bool> OnLaneChangingBlocked;

        private IPool<RoadSpan> _roadFactory;
        private List<RoadSpan> _roadSpans;

        private Vector3 _currentShift;
        private Vector3 _forwardShift = new Vector3(0, 0, 100);
        private Vector3 _leftShift = new Vector3(-100, 0, 0);
        private Vector3 _rightShift = new Vector3(100, 0, 0);

        private RoadSpan _startSpan;
        private List<RoadSpan> _endSpans;
        private int _currentLane;
        private int _roadSpansCreated = 0;
        private int _maxRoadSpans = 8;

        private bool _hasDoubleRoad = false;
        private float _turnTimescale = 1;

        public RouteAnalyzer(Transform firstRoadSpan, IPool<RoadSpan> roadFactory)
        {
            _roadFactory = roadFactory;
            _currentShift = _forwardShift;
            _roadSpans = new List<RoadSpan>();
            _endSpans = new List<RoadSpan>();

            RegisterFirstRoadSpan(firstRoadSpan);
            for (int i = 0; i < _roadFactory.Objects.Count; i++)
                RegisterRoadSpan(_roadFactory.Objects[i]);
        }

        private void RegisterFirstRoadSpan(Transform firstRoadSpan)
        {
            _roadSpansCreated++;
            _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
            _startSpan.Activate();
            _startSpan.OnTurnedOff += OnTurnOffRoad;
            _startSpan.RootObject = _roadFactory.RootObject;
            _endSpans.Add(_startSpan);
        }

        public void OnTurnOffRoad(RoadSpanType roadType)
        {
            if (roadType == RoadSpanType.TwoWays)
                _hasDoubleRoad = false;

            PlanRoadAhead();
        }

        public void PlanRoadAhead()
        {
            int currentEndsNumber = _endSpans.Count;
//#if UNITY_EDITOR
//            if (_endSpans.Count > 2)
//            {
//                UnityEditor.EditorApplication.isPlaying = false;
//                throw new ArgumentException("Not valid number of end spans");
//            }
//#endif
            if (_roadSpansCreated >= _maxRoadSpans)
            {
                UnityEngine.Debug.Log($"Reached a critical number of spawns, new spawn will be skipped");
                return;
            }

            _roadSpansCreated--;
            UnityEngine.Debug.Log($"number of span ends is {_endSpans.Count}:");
            for (int i = 0; i < currentEndsNumber; i++)
            {
                UnityEngine.Debug.Log($"creating from element {i} - {_endSpans[i].name}");

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

            if (currentShift == Vector3.zero)
            {
                _hasDoubleRoad = true;

                var roadSpan2 = SpawnRoadSpan();
                UpdateRoadSpanPosition(roadSpan2, previousSpan, _leftShift);
                _currentShift = _rightShift;
                _endSpans.Add(roadSpan2);
                UnityEngine.Debug.Log($"new left end at index {_endSpans.IndexOf(roadSpan2)} is {roadSpan2.name}");
            }
            UpdateRoadSpanPosition(roadSpan, previousSpan, _currentShift);

            _endSpans[_endSpans.IndexOf(previousSpan)] = roadSpan;
            UnityEngine.Debug.Log($"new (right) end  at index {_endSpans.IndexOf(roadSpan)} is {roadSpan.name}");
        }

        private RoadSpan SpawnRoadSpan(bool shouldSaveSpawn = false)
        {
            RoadSpan roadSpan;

            if (shouldSaveSpawn)
                roadSpan = _roadFactory.Spawn(true);
            else
                roadSpan = _roadFactory.Spawn();

            if (_hasDoubleRoad && roadSpan.RoadType == RoadSpanType.TwoWays)
            {
                _roadFactory.Despawn(roadSpan);
                UnityEngine.Debug.Log("TwoWayRoad was doubled and despawned");
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
            roadSpan.RootObject = _roadFactory.RootObject;
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
                //if (road.RoadType == RoadSpanType.TwoWays)
                //{
                //    UnityEngine.Debug.Log("went int chosing path");
                //    //RoadSpan nextRoadSpan = road.ReturnLeftChild();  // do we need it???
                //    UpdatePreviousSpan(road.RoadType, _currentLane);
                //}
            }
        }

        private void UpdatePreviousSpan(RoadSpanType roadType, int chosenLane) //RoadSpan nextRoadSpan, 
        {
            if (roadType != RoadSpanType.TwoWays)
                return; 

            if (chosenLane == -1)
                _endSpans.Remove(_endSpans[0]);
            else if (chosenLane == 1)
                _endSpans.Remove(_endSpans[1]);
            UnityEngine.Debug.Log($"Chosen {chosenLane} lane, number of end spans is {_endSpans.Count}");
        }

        public void Dispose()
        {
            _startSpan.OnTurnedOff -= OnTurnOffRoad;
            for (int i = 0; i < _roadSpans.Count; i++)
            {
                _roadSpans[i].OnTurnedOff -= OnTurnOffRoad;
                //_roadSpans[i].OnSettingNextRoadSpan -= UpdatePreviousSpan;
                _roadSpans[i].OnTurning -= BlockLaneChanging;
            }
        }
    }
}