using System;

namespace Factories
{
    internal interface IRoadSystem : IDisposable
    {
        IRouteAnalyzer RouteAnalyzer { get; }
        void StartRoadSpawn(int spanNumber);
        void UpdatePlayerLane(int number);
        void SpeedUp(float currentSpeed);
    }
}