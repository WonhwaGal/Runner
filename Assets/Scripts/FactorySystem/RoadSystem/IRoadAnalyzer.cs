using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRoadAnalyzer
    {
        event Action<List<Transform>> RequestForCoins;
        event Action<List<Transform>> RequestForUpdates;
        event Action<bool> OnLaneChangingBlocked;

        void RegisterRoadSpan(RoadSpan roadSpan);
        void UpdatePlayerLane(int number);
        void PlanRoadAhead();
        void Dispose();
    }
}