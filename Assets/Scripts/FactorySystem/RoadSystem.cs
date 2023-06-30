using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class RoadSystem : IRoadSystem
    {
        public event Action<List<Transform>> OnRoadForCoins;
        public event Action<List<Transform>> OnRoadForUpdates;

        private GenericFactory<RoadSpan> _roadFactory;
        private Vector3 _previousPos;
        private Vector3 _forwardShift = new Vector3(0, 0, 100);
        private Vector3 _leftShift = new Vector3(-100, 0, 0);
        private Vector3 _rightShift = new Vector3(100, 0, 0);
        private Vector3 _currentShift;
        private RoadSpan _startSpan;
        private List<RoadSpan> _endSpans;
        private List<RoadSpan> _roadSpans;
        private int _roadSpansSpawned;

        public RoadSystem(Transform firstRoadSpan)
        {
            _roadSpans = new List<RoadSpan>();
            CreateRoadFactory();
            Init(firstRoadSpan);
            UnityEngine.Debug.Log("1st spawn");
            PlanRoadAhead();
            UnityEngine.Debug.Log("2nd spawn");
            PlanRoadAhead();
            UnityEngine.Debug.Log("3rd spawn");
            PlanRoadAhead();
        }

        private void Init(Transform firstRoadSpan)
        {
            _previousPos = firstRoadSpan.position;
            _currentShift = _forwardShift;
            _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
            _startSpan.Activate();
            _startSpan.OnTurnedOff += PlanRoadAhead;
            _startSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _endSpans = new List<RoadSpan>();
            _endSpans.Add(_startSpan);
        }

        private void CreateRoadFactory()
        {
            _roadFactory = new SingleFactory<RoadSpan>("RoadPrefabs");
            AddObjectNamesToTheFactory();
            _roadFactory.CreateListOfObjects();

            for (int i = 0; i < _roadFactory.Objects.Count; i++)
                RegisterRoadSpan(_roadFactory.Objects[i]);
        }

        private void AddObjectNamesToTheFactory()
        {
            _roadFactory.AddPrefabNameToList("RoadBlock1");
            _roadFactory.AddPrefabNameToList("RoadBlockBusStop");
            _roadFactory.AddPrefabNameToList("RoadBlockPark");
            _roadFactory.AddPrefabNameToList("RoadBlockBridge");
            _roadFactory.AddPrefabNameToList("RoadTurnRight");
            _roadFactory.AddPrefabNameToList("RoadTurnDouble");
        }

        private void RegisterRoadSpan(RoadSpan roadSpan)
        {
            roadSpan.OnTurnedOff += PlanRoadAhead;
            roadSpan.OnSettingNextRoadSpan += UpdatePreviousSpan;
            roadSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _roadSpans.Add(roadSpan);

        }

        public void CheckPlayerLane(int number)
        {
            for (int i = 0; i < _roadSpans.Count; i++)
                _roadSpans[i].PlayerLane = number;
        }

        private void UpdatePreviousSpan(RoadSpan roadSpan, int chosenLane)
        {
            if(chosenLane == -1)
            {
                _endSpans.Remove(_endSpans[0]);
                UnityEngine.Debug.Log($"chosen right way");
            }
            else if (chosenLane == 1)
            {
                _endSpans.Remove(_endSpans[1]);
                UnityEngine.Debug.Log($"chosen left way");
            }
            if (_endSpans.Count == 1)
            UnityEngine.Debug.Log($"new only end is {_endSpans[0]}");
        }

        private void PlanRoadAhead()
        {
            int currentEndListState = _endSpans.Count;
            UnityEngine.Debug.Log($"number of span ends is {_endSpans.Count}:");
            for (int i = 0; i < currentEndListState; i++)
            {
                UnityEngine.Debug.Log($"creating from element {i} - {_endSpans[i].name}");
                //_roadSpansSpawned--;
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
            var roadSpan = SpawnRoadSpan(previousSpan.RoadType);

            if (currentShift == Vector3.zero)
            {
                var roadSpan2 = SpawnRoadSpan(previousSpan.RoadType);
                UpdateRoadSpanPosition(roadSpan2, previousSpan, _leftShift);
                _currentShift = _rightShift;
                _endSpans.Add(roadSpan2);
                UnityEngine.Debug.Log($"new left end at index {_endSpans.IndexOf(roadSpan2)} is {roadSpan2.name}");
            }
            UpdateRoadSpanPosition(roadSpan, previousSpan, _currentShift);

            _endSpans[_endSpans.IndexOf(previousSpan)] = roadSpan;
            UnityEngine.Debug.Log($"new (right) end  at index {_endSpans.IndexOf(roadSpan)} is {roadSpan.name}");
        }

        private RoadSpan SpawnRoadSpan(RoadSpanType previousRoadType)
        {
            RoadSpan roadSpan = null;
            bool goodRoadMatch = false;
            while (!goodRoadMatch)
            {
                roadSpan = _roadFactory.Spawn();
                if (previousRoadType != RoadSpanType.Straight && roadSpan.RoadType != RoadSpanType.Straight)
                {
                    roadSpan.Deactivate();
                    Debug.LogWarning($"{roadSpan.name} got deactivated");
                }
                else
                {
                    goodRoadMatch = true;
                }
            }

            if (!_roadSpans.Contains(roadSpan))
                RegisterRoadSpan(roadSpan);
            return roadSpan;
        }

        private void UpdateRoadSpanPosition(RoadSpan currentSpan, RoadSpan previousSpan, Vector3 currentShift)
        {
            _previousPos = previousSpan.transform.position;
            currentSpan.transform.position = _previousPos + currentShift;

            if (currentShift == _rightShift)
                AttachChildRoadSpan(currentSpan.transform, previousSpan, 1);
            else if (currentShift == _leftShift)
                AttachChildRoadSpan(currentSpan.transform, previousSpan, -1);
            else if (currentShift == _forwardShift)
                AttachChildRoadSpan(currentSpan.transform, previousSpan, 0);

            //OnRoadForCoins?.Invoke(roadSpan.CoinSpots);
            //OnRoadForUpdates?.Invoke(roadSpan.UpgradeSpots);
        }

        private void AttachChildRoadSpan(Transform currentSpan, RoadSpan previousSpan, int Xdirection)
        {
            if (Xdirection != 0)
                currentSpan.transform.forward = Vector3.right * Xdirection;
            else
            {
                currentSpan.transform.forward = previousSpan.transform.forward;

                if (previousSpan.transform.forward == Vector3.right)
                    currentSpan.transform.position = previousSpan.transform.position + _rightShift;

                if (previousSpan.transform.forward == Vector3.left)
                    currentSpan.transform.position = previousSpan.transform.position + _leftShift;
            }

            previousSpan.AcceptAChildObject(currentSpan);
        }

        public void Dispose()
        {
            _startSpan.OnTurnedOff -= PlanRoadAhead;
            for (int i = 0; i < _roadFactory.Objects.Count; i++)
            {
                _roadFactory.Objects[i].OnTurnedOff -= PlanRoadAhead;
                _roadFactory.Objects[i].OnSettingNextRoadSpan -= UpdatePreviousSpan;
            }
        }
    }
}