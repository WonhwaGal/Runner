using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class RoadAnalyzer : IRoadAnalyzer
    {
        public event Action<List<Transform>> RequestForCoins;
        public event Action<List<Transform>> RequestForUpdates;
        public event Action<bool> OnLaneChangingBlocked;

        GenericFactory<RoadSpan> _roadFactory;
        private List<RoadSpan> _roadSpans;

        private Vector3 _currentShift;
        private Vector3 _forwardShift = new Vector3(0, 0, 100);
        private Vector3 _leftShift = new Vector3(-100, 0, 0);
        private Vector3 _rightShift = new Vector3(100, 0, 0);

        private RoadSpan _startSpan;
        private List<RoadSpan> _endSpans;
        private int _maxRoadSpansSpawned = 5;
        private int _roadSpansSpawned;

        public RoadAnalyzer(Transform firstRoadSpan, GenericFactory<RoadSpan> roadFactory)
        {
            _roadFactory = roadFactory;
            _currentShift = _forwardShift;
            _roadSpans = new List<RoadSpan>();
            _endSpans = new List<RoadSpan>();
            _roadSpansSpawned = _maxRoadSpansSpawned;

            RegisterFirstRoadSpan(firstRoadSpan);
            for (int i = 0; i < _roadFactory.Objects.Count; i++)
                RegisterRoadSpan(_roadFactory.Objects[i]);
        }

        private void RegisterFirstRoadSpan(Transform firstRoadSpan)
        {
            _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
            _startSpan.Activate();
            _startSpan.OnTurnedOff += PlanRoadAhead;
            _startSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _endSpans.Add(_startSpan);
        }

        public void UpdatePlayerLane(int number)
        {
            for (int i = 0; i < _roadSpans.Count; i++)
                _roadSpans[i].PlayerLane = number;
        }

        public void RegisterRoadSpan(RoadSpan roadSpan)
        {
            roadSpan.OnTurning += BlockLaneChanging;
            roadSpan.OnSettingNextRoadSpan += UpdatePreviousSpan;
            roadSpan.OnTurnedOff += PlanRoadAhead;
            roadSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _roadSpans.Add(roadSpan);
        }

        private void BlockLaneChanging(bool shouldBlock) => OnLaneChangingBlocked?.Invoke(shouldBlock);

        private void UpdatePreviousSpan(RoadSpan roadSpan, int chosenLane)
        {
            if (chosenLane == -1)
                _endSpans.Remove(_endSpans[0]);
            else if (chosenLane == 1)
                _endSpans.Remove(_endSpans[1]);
            UnityEngine.Debug.Log($"Chosen {chosenLane} lane, number of end spans is {_endSpans.Count}");
        }

        public void PlanRoadAhead()
        {
            _roadSpansSpawned--;
            //if (_roadSpansSpawned > _maxRoadSpansSpawned)
            //return;

            int currentEndListState = _endSpans.Count;
            if (_endSpans.Count > 2)
                throw new ArgumentException("Not valid number of end spans");

            UnityEngine.Debug.Log($"number of span ends is {_endSpans.Count}:");
            for (int i = 0; i < currentEndListState; i++)
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

        private RoadSpan SpawnRoadSpan()
        {
            RoadSpan roadSpan = _roadFactory.Spawn();

            if (!_roadSpans.Contains(roadSpan))
                RegisterRoadSpan(roadSpan);
            _roadSpansSpawned++;

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

            //OnRoadForCoins?.Invoke(roadSpan.CoinSpots);
            //OnRoadForUpdates?.Invoke(roadSpan.UpgradeSpots);
        }

        private void PlaceAsChildObject(RoadSpan currentSpan, RoadSpan previousSpan, int direction, Vector3 currentShift)
        {
            currentSpan.transform.position = previousSpan.transform.position;
            currentSpan.transform.rotation = previousSpan.transform.rotation;

            currentSpan.transform.rotation *= Quaternion.Euler(0, 90 * direction, 0);
            previousSpan.AcceptAChildObject(currentSpan.transform);
            currentSpan.transform.localPosition += currentShift;
        }

        public void Dispose()
        {
            _startSpan.OnTurnedOff -= PlanRoadAhead;
            for (int i = 0; i < _roadSpans.Count; i++)
            {
                _roadSpans[i].OnTurnedOff -= PlanRoadAhead;
                _roadSpans[i].OnSettingNextRoadSpan -= UpdatePreviousSpan;
                _roadSpans[i].OnTurning -= BlockLaneChanging;
            }
        }
    }
}