using System;
using UnityEngine;
using Collectables;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal class TriggerHandler
    {
        private readonly PlayerUpgradeController _upgrader;
        private float _playerUpgradeMultiplier;
        private int _playerCoinMultiplier;
        private int _playerCrystalMultiplier;

        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;

        public PlayerUpgradeController Upgrader => _upgrader;

        public Action<float, UpgradeType> OnGettingUpgrade { get; set; }
        public Action OnHittingAnObstacle { get; set; }

        public void Init(PlayerConfig config)
        {
            _playerUpgradeMultiplier = config.UpgradeMultiplier;
            _playerCoinMultiplier = config.CoinMultiplier;
            _playerCrystalMultiplier = config.CrystalMultiplier;
        }

        public void SortOutCollectable(CollectableObject collectable)
        {
            float value;
            switch (collectable.Type)
            {
                case CollectableType.Coin:
                    value = collectable.Value * Upgrader.CoinMultiplier * _playerCoinMultiplier;
                    GameEventSystem.Send(new CollectEvent(collectable.Type, (int)value));
                    break;
                case CollectableType.Crystal:
                    value = _playerCrystalMultiplier * Upgrader.CrystalMultiplier;
                    GameEventSystem.Send(new CollectEvent(collectable.Type, (int)value));
                    break;
                default:
                    value = collectable.Value * _playerUpgradeMultiplier;
                    OnGettingUpgrade?.Invoke(value, collectable.Upgrade);
                    GameEventSystem.Send(new UpgradeEvent(value, collectable.Upgrade));
                    break;
            }
        }

        public void RegisterObstacleHit(Collider other)
        {
            if (other.CompareTag("NoPass") || !Upgrader.CheckShield())
            {
                OnHittingAnObstacle?.Invoke();
                GameEventSystem.Send(new UpgradeEvent(0, UpgradeType.None));
            }
        }
    }
}