using System;

namespace Factories
{
    internal interface IRoadSystem : IDisposable
    {
        IRouteAnalyzer RouteAnalyzer { get; }
        void StartRoadSpawn();
        void UpdatePlayerLane(int number);
        void SpeedUp(float currentSpeed);
    }
}