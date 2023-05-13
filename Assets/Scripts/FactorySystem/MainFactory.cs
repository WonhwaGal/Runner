using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace Factories
{
    internal class MainFactory
    {
        private List<Timer> _timers;
        private RoadSystem _roadSystem;
        private CoinSetSystem _coinSetSystem;
        public MainFactory(List<Timer> timers, Transform firstRoadSpan)
        {
            _timers = timers;
            _roadSystem = new RoadSystem(_timers, firstRoadSpan);
            _coinSetSystem = new CoinSetSystem();
            _roadSystem.OnBuildingRoadSpan += _coinSetSystem.PutCoinsOnRoad;
        }

        public void Dispose()
        {
            _roadSystem.OnBuildingRoadSpan -= _coinSetSystem.PutCoinsOnRoad;
        }

    }
}