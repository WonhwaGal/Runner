using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class RoadSystem : IRoadSystem
    {
        public event Action<List<Transform>> OnRoadForCoins;
        public event Action<List<Transform>> OnRoadForUpdates;
        public event Action<bool> OnLaneChangingBlocked;

        private GenericFactory<RoadSpan> _roadFactory;
        private Vector3 _parentPos;
        private Quaternion _parentRot;

        private Vector3 _forwardShift = new Vector3(0, 0, 100);
        private Vector3 _leftShift = new Vector3(-100, 0, 0);
        private Vector3 _rightShift = new Vector3(100, 0, 0);
        private Vector3 _currentShift;

        private RoadSpan _startSpan;
        private List<RoadSpan> _endSpans;
        private List<RoadSpan> _roadSpans;
        private int _maxRoadSpansSpawned = 5;
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
            //UnityEngine.Debug.Log("3rd spawn");
            //PlanRoadAhead();
        }

        private void Init(Transform firstRoadSpan)
        {
            _parentPos = firstRoadSpan.position;
            _currentShift = _forwardShift;
            _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
            _startSpan.Activate();
            _startSpan.OnTurnedOff += PlanRoadAhead;
            _startSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _endSpans = new List<RoadSpan>();
            _endSpans.Add(_startSpan);
            _roadSpansSpawned = _maxRoadSpansSpawned;
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
            roadSpan.OnTurning += BlockLaneChanging;
            roadSpan.FactoryParentTransfom = _roadFactory.RootObject;
            _roadSpans.Add(roadSpan);
        }

        public void BlockLaneChanging(bool shouldBlock) => OnLaneChangingBlocked?.Invoke(shouldBlock);

        public void CheckPlayerLane(int number)
        {
            for (int i = 0; i < _roadSpans.Count; i++)
                _roadSpans[i].PlayerLane = number;
        }

        private void UpdatePreviousSpan(RoadSpan roadSpan, int chosenLane)
        {
            if(chosenLane == -1)
                _endSpans.Remove(_endSpans[0]);
            else if (chosenLane == 1)
                _endSpans.Remove(_endSpans[1]);
        }

        private void PlanRoadAhead()
        {
            _roadSpansSpawned--;
            //if (_roadSpansSpawned > _maxRoadSpansSpawned)
                //return;

            int currentEndListState = _endSpans.Count;
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
            if (!goodRoadMatch)
            {
                roadSpan = _roadFactory.Spawn();
                if (previousRoadType != RoadSpanType.Straight && roadSpan.RoadType != RoadSpanType.Straight)
                {
                    roadSpan.Deactivate();
                    Debug.LogWarning($"{roadSpan.name} got deactivated");
                    SpawnRoadSpan(previousRoadType);
                }
            }

            if (!_roadSpans.Contains(roadSpan))
                RegisterRoadSpan(roadSpan);
            _roadSpansSpawned++;
            return roadSpan;
        }

        private void UpdateRoadSpanPosition(RoadSpan currentSpan, RoadSpan previousSpan, Vector3 currentShift)
        {
            _parentPos = previousSpan.transform.position;
            _parentRot = previousSpan.transform.rotation;

            if(currentShift == _rightShift)
            {
                currentSpan.transform.rotation = _parentRot;
                currentSpan.transform.rotation *= Quaternion.Euler(0, 90, 0);
                previousSpan.AcceptAChildObject(currentSpan.transform);
                currentSpan.transform.position = _parentPos;
                currentSpan.transform.localPosition += _rightShift;
            }
            else if (currentShift == _leftShift)
            {
                currentSpan.transform.rotation = _parentRot;
                currentSpan.transform.rotation *= Quaternion.Euler(0, -90, 0);
                previousSpan.AcceptAChildObject(currentSpan.transform);
                currentSpan.transform.position = _parentPos;
                currentSpan.transform.localPosition += _leftShift;
            }
            else if (currentShift == _forwardShift)
            {
                currentSpan.transform.rotation = _parentRot;
                previousSpan.AcceptAChildObject(currentSpan.transform);
                currentSpan.transform.position = _parentPos;
                currentSpan.transform.localPosition += _forwardShift;
            }

            //OnRoadForCoins?.Invoke(roadSpan.CoinSpots);
            //OnRoadForUpdates?.Invoke(roadSpan.UpgradeSpots);
        }

        public void Dispose()
        {
            _startSpan.OnTurnedOff -= PlanRoadAhead;
            for (int i = 0; i < _roadFactory.Objects.Count; i++)
            {
                _roadFactory.Objects[i].OnTurnedOff -= PlanRoadAhead;
                _roadFactory.Objects[i].OnSettingNextRoadSpan -= UpdatePreviousSpan;
                _roadFactory.Objects[i].OnTurning -= BlockLaneChanging;
            }
        }
    }
}