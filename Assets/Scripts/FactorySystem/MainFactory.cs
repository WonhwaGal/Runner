using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Factories
{
    internal class MainFactory
    {
        private RoadSystem _roadSystem;
        private CoinSetSystem _coinSetSystem;
        public MainFactory(Transform firstRoadSpan)
        {
            _roadSystem = new RoadSystem(firstRoadSpan);
            _coinSetSystem = new CoinSetSystem();
            _roadSystem.OnBuildingRoadSpan += _coinSetSystem.PutCoinsOnRoad;
        }

        public void Dispose()
        {
            _roadSystem.OnBuildingRoadSpan -= _coinSetSystem.PutCoinsOnRoad;
        }

    }
}