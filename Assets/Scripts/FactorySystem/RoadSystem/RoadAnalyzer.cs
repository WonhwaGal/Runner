using Factories;
using System.Collections.Generic;
using UnityEngine;

public class RoadAnalyzer
{
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

    //public RoadAnalyzer(Transform firstRoadSpan)
    //{
    //    _parentPos = firstRoadSpan.position;
    //    _currentShift = _forwardShift;
    //    _startSpan = firstRoadSpan.gameObject.GetComponent<RoadSpan>();
    //    _startSpan.Activate();
    //    _startSpan.OnTurnedOff += PlanRoadAhead;
    //    _startSpan.FactoryParentTransfom = _roadFactory.RootObject;
    //    _endSpans = new List<RoadSpan>();
    //    _endSpans.Add(_startSpan);
    //    _roadSpansSpawned = _maxRoadSpansSpawned;
    //}

    //private void RegisterRoadSpan(RoadSpan roadSpan)
    //{
    //    roadSpan.OnTurnedOff += PlanRoadAhead;
    //    roadSpan.OnSettingNextRoadSpan += UpdatePreviousSpan;
    //    roadSpan.OnTurning += BlockLaneChanging;
    //    roadSpan.FactoryParentTransfom = _roadFactory.RootObject;
    //    _roadSpans.Add(roadSpan);
    //}
}
