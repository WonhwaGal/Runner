
namespace Factories
{
    internal interface IRoadSystem
    {
        IRouteAnalyzer RouteAnalyzer { get; }
        void StartRoadSpawn();
        void UpdatePlayerLane(int number);
        void Dispose();
    }
}