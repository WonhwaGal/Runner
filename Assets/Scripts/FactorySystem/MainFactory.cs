using UnityEngine;


namespace Factories
{
    internal class MainFactory: IMainFactory
    {
        private IRoadSystem _roadSystem;
        private ICoinSetSystem _coinSetSystem;
        private IUpgradeSpawnSystem _upgradeSpawnSystem;

        public IRoadSystem RoadSystem { get => _roadSystem; }
        public ICoinSetSystem CoinSetSystem { get => _coinSetSystem; }

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
            _roadSystem.Dispose();
        }
    }
}