using Collectables;
using System;

namespace PlayerSystem
{
    internal class TriggerHandler
    {
        private PlayerUpgradeController _upgrader;

        public Action<int> OnTriggeredByCoin;
        public Action<float, UpgradeType> OnGettingUpgrade;
        public Action OnHittingAnObstacle;
        public TriggerHandler(PlayerUpgradeController upgrader) => _upgrader = upgrader;

        public void SortOutCollectable(ICollectable collectable)
        {
            if (collectable.Type == CollectableType.Coin)
                OnTriggeredByCoin?.Invoke(collectable.Value * _upgrader.CoinMultiplier);
            else if (collectable.Type == CollectableType.Upgrade)
                OnGettingUpgrade?.Invoke(collectable.Value, collectable.Upgrade);
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