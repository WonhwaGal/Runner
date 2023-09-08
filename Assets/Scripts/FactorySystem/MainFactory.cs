using UnityEngine;


using System;

namespace Factories
{
    internal class MainFactory : IDisposable
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
            RoadSystem.RouteAnalyzer.RequestForCoins += CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.RouteAnalyzer.RequestForUpgrades += _upgradeSpawnSystem.PutUpgradesOnRoad;

            _roadSystem.StartRoadSpawn();
        }


        public void UpdateAnimations(bool isPaused)
        {
            _coinSetSystem.UpdateAnimationState(isPaused);
            _upgradeSpawnSystem.UpdateAnimationState(isPaused);
        }

        public void Dispose()
        {
            RoadSystem.RouteAnalyzer.RequestForCoins -= CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.RouteAnalyzer.RequestForUpgrades -= _upgradeSpawnSystem.PutUpgradesOnRoad;
            //_roadSystem.Dispose();
        }
    }
}