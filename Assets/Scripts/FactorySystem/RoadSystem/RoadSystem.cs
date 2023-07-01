using UnityEngine;


namespace Factories
{
    internal class RoadSystem : IRoadSystem
    {
        private GenericFactory<RoadSpan> _roadFactory;

        private IRoadAnalyzer _roadAnalyzer;

        public IRoadAnalyzer RoadAnalyzer { get => _roadAnalyzer; }

        public RoadSystem(Transform firstRoadSpan)
        {
            CreateRoadFactory();

            _roadAnalyzer = new RoadAnalyzer(firstRoadSpan, _roadFactory);

            RoadAnalyzer.PlanRoadAhead();
            RoadAnalyzer.PlanRoadAhead();
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
            _roadFactory.AddPrefabNameToList("RoadTurnDouble");
        }

        public void UpdatePlayerLane(int number)
        {
            RoadAnalyzer.UpdatePlayerLane(number);
        }

        public void Dispose() => RoadAnalyzer.Dispose();
    }
}