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

        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;
        public void Init(PlayerConfig config)
        {
            _playerUpgradeMultiplier = config.UpgradeMultiplier;
            _playerCoinMultiplier = config.CoinMultiplier;
            _playerCrystalMultiplier = config.CrystalMultiplier;
        }

        public void SendMagnetController(PlayerMagnetController playerMagnetCollider) 
            => _upgrader.AddMagnetController(playerMagnetCollider);

        public void SortOutCollectable(CollectableObject collectable)
        {
            if (collectable.Type == CollectableType.Coin)
                OnTriggeredByCoin?.Invoke(collectable.Value * _upgrader.CoinMultiplier * _playerCoinMultiplier);
            else if (collectable.Type == CollectableType.Upgrade)
                OnGettingUpgrade?.Invoke(collectable.Value * _playerUpgradeMultiplier, collectable.Upgrade);
            else if (collectable.Type == CollectableType.Crystal)
                OnTriggeredByCrystal?.Invoke(_playerCrystalMultiplier * _upgrader.CrystalMultiplier);
        }

        public void RegisterObstacleHit()
        {
            if (_upgrader.CheckShield()) 
            {
                UnityEngine.Debug.Log("shield is ON");
            }
            else
            {
                UnityEngine.Debug.Log("shield down");
                OnHittingAnObstacle?.Invoke();
            }
        }
    }
}