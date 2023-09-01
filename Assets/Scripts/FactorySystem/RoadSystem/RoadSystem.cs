using UnityEngine;


namespace Factories
{
    internal class RoadSystem : IRoadSystem
    {
        private IPool<RoadSpan> _roadFactory;

        private IRouteAnalyzer _roadAnalyzer;

        public IRouteAnalyzer RouteAnalyzer { get => _roadAnalyzer; }

        public RoadSystem(Transform firstRoadSpan)
        {
            CreateRoadFactory();

            _roadAnalyzer = new RouteAnalyzer(firstRoadSpan, _roadFactory);
        }

        public void StartRoadSpawn()
        {
            RouteAnalyzer.PlanRoadAhead();
            RouteAnalyzer.PlanRoadAhead();
        }

        private void CreateRoadFactory()
        {
            //создаем и настраиваем фабрику объектов
            var roadFactory = new GenericFactory<RoadSpan>("RoadPrefabs");
            roadFactory.LoadPrefab("RoadBlock1");
            roadFactory.LoadPrefab("RoadBlockBusStop");
            roadFactory.LoadPrefab("RoadBlockPark");
            roadFactory.LoadPrefab("RoadBlockBridge");
            roadFactory.LoadPrefab("RoadTurnRight");
            roadFactory.LoadPrefab("RoadTurnLeft");
            roadFactory.LoadPrefab("RoadTurnDouble");

            //создаем пулл объектов
            _roadFactory = new SingleFactory<RoadSpan>(roadFactory);
        }

        public void UpdatePlayerLane(int number) => RouteAnalyzer.UpdatePlayerLane(number);

        public void Dispose() => RouteAnalyzer.Dispose();
    }
}