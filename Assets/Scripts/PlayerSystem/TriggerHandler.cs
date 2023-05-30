using Collectables;
using System;

namespace PlayerSystem
{
    internal class TriggerHandler
    {
        public Action<int> OnTriggeredByCoin { get; set; }
        public Action OnTriggeredByCrystal { get; set; }
        public Action<float, UpgradeType> OnGettingUpgrade;
        public Action OnHittingAnObstacle;

        private PlayerUpgradeController _upgrader;
        private float _playerUpgradeMultiplier;
        private int _playerCoinMultiplier;


        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;
        public void Init(float upgradeM, int coinM)
        {
            _playerUpgradeMultiplier = upgradeM;
            _playerCoinMultiplier = coinM;
        }

        public void SortOutCollectable(CollectableObject collectable)
        {
            if (collectable.Type == CollectableType.Coin)
                OnTriggeredByCoin?.Invoke(collectable.Value * _upgrader.CoinMultiplier * _playerCoinMultiplier);
            else if (collectable.Type == CollectableType.Upgrade)
                OnGettingUpgrade?.Invoke(collectable.Value * _playerUpgradeMultiplier, collectable.Upgrade);
            else if (collectable.Type == CollectableType.Crystal)
                OnTriggeredByCrystal?.Invoke();

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