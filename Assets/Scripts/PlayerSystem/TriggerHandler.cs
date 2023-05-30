using Collectables;
using System;

namespace PlayerSystem
{
    internal class TriggerHandler
    {
        public Action<int> OnTriggeredByCoin { get; set; }
        public Action<int> OnTriggeredByCrystal { get; set; }
        public Action<float, UpgradeType> OnGettingUpgrade { get; set; }
        public Action OnHittingAnObstacle;

        private PlayerUpgradeController _upgrader;
        private float _playerUpgradeMultiplier;
        private int _playerCoinMultiplier;
        private int _playerCrystalMultiplier;

        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;
        public void Init(float upgradeM, int coinM, int crystalM)
        {
            _playerUpgradeMultiplier = upgradeM;
            _playerCoinMultiplier = coinM;
            _playerCrystalMultiplier = crystalM;
        }

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