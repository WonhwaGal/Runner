using System;
using UnityEngine;

namespace Factories
{
    internal class MainFactory : IDisposable
    {
        private readonly IRoadSystem _roadSystem;
        private readonly ICoinSetSystem _coinSetSystem;
        private readonly IUpgradeSpawnSystem _upgradeSpawnSystem;

        public MainFactory(Transform firstRoadSpan)
        {
            _roadSystem = new RoadSystem(firstRoadSpan);
            _coinSetSystem = new CoinSetSystem();
            _upgradeSpawnSystem = new UpgradeSpawnSystem();
            RoadSystem.RouteAnalyzer.RequestForCoins += CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.RouteAnalyzer.RequestForUpgrades += _upgradeSpawnSystem.PutUpgradesOnRoad;
            GameEventSystem.Subscribe<PauseGameEvent>(UpdateAnimations);

            _roadSystem.StartRoadSpawn(3);
        }

        public IRoadSystem RoadSystem => _roadSystem;
        public ICoinSetSystem CoinSetSystem => _coinSetSystem;

        public void UpdateAnimations(PauseGameEvent pauseEvent)
        {
            _coinSetSystem.UpdateAnimationState(pauseEvent.IsPaused);
            _upgradeSpawnSystem.UpdateAnimationState(pauseEvent.IsPaused);
        }

        public void Dispose()
        {
            RoadSystem.RouteAnalyzer.RequestForCoins -= CoinSetSystem.PutCoinsOnRoad;
            RoadSystem.RouteAnalyzer.RequestForUpgrades -= _upgradeSpawnSystem.PutUpgradesOnRoad;
            _roadSystem.Dispose();
        }
    }
}