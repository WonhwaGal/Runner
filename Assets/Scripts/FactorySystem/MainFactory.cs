using UnityEngine;


namespace Factories
{
    internal class MainFactory
    {
        private RoadSystem _roadSystem;
        private CoinSetSystem _coinSetSystem;
        private UpgradeSpawnSystem _upgradeSpawnSystem;
        public RoadSystem RoadSystem { get => _roadSystem; }
        public CoinSetSystem CoinSetSystem { get => _coinSetSystem; }

        public MainFactory(Transform firstRoadSpan)
        {
            _roadSystem = new RoadSystem(firstRoadSpan);
            _coinSetSystem = new CoinSetSystem();
            _upgradeSpawnSystem = new UpgradeSpawnSystem();
            RoadSystem.OnRoadForCoins += CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.OnRoadForUpdates += _upgradeSpawnSystem.PutUpgradesOnRoad;
        }
        public void UpdateAnimations(bool isPaused)
        {
            _coinSetSystem.UpdateAnimationState(isPaused);
            _upgradeSpawnSystem.UpdateAnimationState(isPaused);
        }

        public void Dispose()
        {
            RoadSystem.OnRoadForCoins -= CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.OnRoadForCoins -= _upgradeSpawnSystem.PutUpgradesOnRoad;
        }
    }
}