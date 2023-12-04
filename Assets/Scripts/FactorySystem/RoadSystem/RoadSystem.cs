using UnityEngine;

namespace Factories
{
    internal sealed class RoadSystem : IRoadSystem
    {
        private IPool<RoadSpan> _roadPool;
        private readonly IRouteAnalyzer _roadAnalyzer;

        public RoadSystem(Transform firstRoadSpan)
        {
            CreateRoadFactory();
            _roadAnalyzer = new RouteAnalyzer(firstRoadSpan, _roadPool);
        }

        public IRouteAnalyzer RouteAnalyzer => _roadAnalyzer;

        public void StartRoadSpawn(int spanNumber)
        {
            for(int i = 0; i < spanNumber; i++)
                RouteAnalyzer.PlanRoadAhead();
        }

        private void CreateRoadFactory()
        {
            var roadFactory = new GenericFactory<RoadSpan>("RoadPrefabs");
            roadFactory.LoadPrefab("RoadBlock1");
            roadFactory.LoadPrefab("RoadBlockBusStop");
            roadFactory.LoadPrefab("RoadBlockPark");
            roadFactory.LoadPrefab("RoadBlockBridge");
            roadFactory.LoadPrefab("RoadTurnRight");
            roadFactory.LoadPrefab("RoadTurnLeft");
            roadFactory.LoadPrefab("RoadTurnDouble");

            _roadPool = new SinglePool<RoadSpan>(roadFactory);
            _roadPool.PrespawnAllPrefabs();
        }

        public void UpdatePlayerLane(int number) => RouteAnalyzer.UpdatePlayerLane(number);

        public void SpeedUp(float currentSpeed) => RouteAnalyzer.SetTurnTimescale(currentSpeed);

        public void Dispose() => RouteAnalyzer.Dispose();
    }
}