using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class UpgradeSpawnSystem: IUpgradeSpawnSystem
    {
        private IPool<UpgradeView> _upgradePool;

        public UpgradeSpawnSystem() => CreateUpgradesFactory();

        private void CreateUpgradesFactory()
        {
            var upgradeFactory = new GenericFactory<UpgradeView>("UpgradePrefabs");
            upgradeFactory.LoadPrefab("DoubleUpgrade");
            upgradeFactory.LoadPrefab("ShieldUpgrade");
            upgradeFactory.LoadPrefab("JemCollectable");
            upgradeFactory.LoadPrefab("MagnetUpgrade");

            _upgradePool = new SinglePool<UpgradeView>(upgradeFactory);
        }

        public void UpdateAnimationState(bool isPaused)
        {
            for (int i = 0; i < _upgradePool.ActiveObjects.Count; i++)
                _upgradePool.ActiveObjects[i].PauseChild(isPaused);
        }

        public void PutUpgradesOnRoad(IRoadSpan roadSpan)
        {
            for (int i = 0; i < GetRandomProbability(roadSpan.UpgradeSpots); i++)
            {
                var YShiftVector = new Vector3(0, Random.Range(1.0f, 3.0f), 0);
                UpgradeView upgradeView = _upgradePool.Spawn();
                upgradeView.transform.position = roadSpan.UpgradeSpots[i].position + YShiftVector;
                roadSpan.AcceptChildRespawnable(upgradeView, RespawnableType.Upgrade);
            }
        }

        private int GetRandomProbability(List<Transform> upgradeSpots)
        {
            int returnOfThreeUpgrades = 10;
            int returnOfTwoUpgrades = 35;
            int returnOfOneUpgrade = 80;
            int  result = 0;

            int randomnumber = Random.Range(0, 100);
            if (randomnumber <= returnOfThreeUpgrades)
                result = returnOfTwoUpgrades;
            else if (randomnumber <= returnOfTwoUpgrades)
                result = returnOfTwoUpgrades;
            else if (randomnumber <= returnOfOneUpgrade)
                result = returnOfOneUpgrade;

            return result switch
            {
                10 => upgradeSpots.Count,
                35 => upgradeSpots.Count - 1,
                80 => upgradeSpots.Count - 2,
                _ => 0,
            };
        }


    }
}