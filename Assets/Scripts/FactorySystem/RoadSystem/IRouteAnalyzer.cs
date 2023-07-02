using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRouteAnalyzer
    {
        event Action<IRoadSpan> RequestForCoins;
        event Action<IRoadSpan> RequestForUpgrades;
        event Action<bool> OnLaneChangingBlocked;

        void RegisterRoadSpan(RoadSpan roadSpan);
        void UpdatePlayerLane(int number);
        void PlanRoadAhead();
        void CheckForTurn(RoadSpan roadSpan);
        void Dispose();
    }
}