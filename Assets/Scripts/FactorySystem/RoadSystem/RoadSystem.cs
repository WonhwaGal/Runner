using UnityEngine;


namespace Factories
{
    internal class RoadSystem : IRoadSystem
    {
        private GenericFactory<RoadSpan> _roadFactory;

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
            _roadFactory = new SingleFactory<RoadSpan>("RoadPrefabs");
            AddObjectNamesToTheFactory();
            _roadFactory.CreateListOfObjects();
        }

        private void AddObjectNamesToTheFactory()
        {
            _roadFactory.AddPrefabNameToList("RoadBlock1");
            _roadFactory.AddPrefabNameToList("RoadBlockBusStop");
            _roadFactory.AddPrefabNameToList("RoadBlockPark");
            _roadFactory.AddPrefabNameToList("RoadBlockBridge");
            _roadFactory.AddPrefabNameToList("RoadTurnRight");
            _roadFactory.AddPrefabNameToList("RoadTurnLeft");
            _roadFactory.AddPrefabNameToList("RoadTurnDouble");
        }

        public void UpdatePlayerLane(int number) => RouteAnalyzer.UpdatePlayerLane(number);

        public void Dispose() => RouteAnalyzer.Dispose();
    }
}