using UnityEngine;


namespace Factories
{
    internal class MainFactory
    {
        private RoadSystem _roadSystem;
        private CoinSetSystem _coinSetSystem;
        private UpgradeSpawnSystem _upgradeSpawnSystem;
        public RoadSystem RoadSystem { get => _roadSystem; }

        public MainFactory(Transform firstRoadSpan)
        {
            _roadSystem = new RoadSystem(firstRoadSpan);
            _coinSetSystem = new CoinSetSystem();
            _upgradeSpawnSystem = new UpgradeSpawnSystem();
            RoadSystem.OnRoadForCoins += _coinSetSystem.PutCoinsOnRoad;
            RoadSystem.OnRoadForUpdates += _upgradeSpawnSystem.PutUpgradesOnRoad;
        }

        public void Dispose()
        {
            RoadSystem.OnRoadForCoins -= _coinSetSystem.PutCoinsOnRoad;
            RoadSystem.OnRoadForCoins -= _upgradeSpawnSystem.PutUpgradesOnRoad;
        }
    }
}