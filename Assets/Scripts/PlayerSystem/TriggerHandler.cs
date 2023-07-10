using Collectables;
using System;
using UnityEngine;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal class TriggerHandler
    {
        public Action<int> OnTriggeredByCoin { get; set; }
        public Action<int> OnTriggeredByCrystal { get; set; }
        public Action<float, UpgradeType> OnGettingUpgrade { get; set; }
        public Action OnHittingAnObstacle { get; set; }

        private PlayerUpgradeController _upgrader;
        private float _playerUpgradeMultiplier;
        private int _playerCoinMultiplier;
        private int _playerCrystalMultiplier;

        public PlayerUpgradeController Upgrader { get => _upgrader;}

        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;

        public void Init(PlayerConfig config)
        {
            _playerUpgradeMultiplier = config.UpgradeMultiplier;
            _playerCoinMultiplier = config.CoinMultiplier;
            _playerCrystalMultiplier = config.CrystalMultiplier;
        }

        public void SortOutCollectable(CollectableObject collectable)
        {
            if (collectable.Type == CollectableType.Coin)
                OnTriggeredByCoin?.Invoke(collectable.Value * Upgrader.CoinMultiplier * _playerCoinMultiplier);
            else if (collectable.Type == CollectableType.Upgrade)
                OnGettingUpgrade?.Invoke(collectable.Value * _playerUpgradeMultiplier, collectable.Upgrade);
            else if (collectable.Type == CollectableType.Crystal)
                OnTriggeredByCrystal?.Invoke(_playerCrystalMultiplier * Upgrader.CrystalMultiplier);
        }

        public void RegisterObstacleHit(Collider other)
        {
            if(other.CompareTag("NoPass") || !Upgrader.CheckShield())
                OnHittingAnObstacle?.Invoke();
        }
    }
}