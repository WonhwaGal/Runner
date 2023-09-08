using System;


namespace Factories
{
    internal interface IRouteAnalyzer : IDisposable
    {
        event Action<IRoadSpan> RequestForCoins;
        event Action<IRoadSpan> RequestForUpgrades;
        event Action<bool> OnLaneChangingBlocked;

        void RegisterRoadSpan(RoadSpan roadSpan);
        void UpdatePlayerLane(int number);
        void PlanRoadAhead();
        void CheckForTurn(RoadSpan roadSpan);
        void SetTurnTimescale(float newTimescale);
    }
}