using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class UpgradeSpawnSystem: IUpgradeSpawnSystem
    {
        private GenericFactory<UpgradeView> _upgradeFactory;

        public UpgradeSpawnSystem() => CreateUpgradesFactory();


        private void CreateUpgradesFactory()
        {
            _upgradeFactory = new SingleFactory<UpgradeView>("UpgradePrefabs");
            AddObjectsToTheFactory();
            _upgradeFactory.CreateListOfObjects();
        }

        private void AddObjectsToTheFactory()
        {
            _upgradeFactory.AddPrefabNameToList("DoubleUpgrade");
            _upgradeFactory.AddPrefabNameToList("ShieldUpgrade");
            _upgradeFactory.AddPrefabNameToList("JemCollectable");
            _upgradeFactory.AddPrefabNameToList("MagnetUpgrade");
        }

        public void UpdateAnimationState(bool isPaused)
        {
            for (int i = 0; i < _upgradeFactory.Objects.Count; i++)
                _upgradeFactory.Objects[i].PauseChild(isPaused);
        }

        public void PutUpgradesOnRoad(List<Transform> upgradeSpots)
        {
            for (int i = 0; i < GetRandomProbability(upgradeSpots); i++)
            {
                var YShiftVector = new Vector3(0, Random.Range(1.0f, 3.0f), 0);
                _upgradeFactory.Spawn().transform.position = upgradeSpots[i].position + YShiftVector;
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