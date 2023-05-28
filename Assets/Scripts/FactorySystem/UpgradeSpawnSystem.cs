using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    public class UpgradeSpawnSystem
    {
        private GenericFactory<UpgradeView> _upgradeFactory;

        public UpgradeSpawnSystem() => CreateUpgradesFactory();

        private void CreateUpgradesFactory()
        {
            _upgradeFactory = new SingleFactory<UpgradeView>("UpgradePrefabs");
            _upgradeFactory.AddPrefabNameToList("DoubleUpgrade");
            _upgradeFactory.AddPrefabNameToList("ShieldUpgrade");

            _upgradeFactory.CreateListOfObjects();
        }

        public void UpdateAnimationState(bool isPaused)
        {
            for (int i = 0; i < _upgradeFactory.Objects.Count; i++)
                _upgradeFactory.Objects[i].PauseChild(isPaused);
        }
        public void PutUpgradesOnRoad(List<Transform> upgradeSpots)
        {
            for (int i = 0; i < GetNumberOfUpgrades(upgradeSpots.Count); i++)
            {
                var YShiftVector = new Vector3(0, Random.Range(1.0f, 3.0f), 0);
                _upgradeFactory.Spawn().transform.position = upgradeSpots[i].position + YShiftVector;
            }
        }
        private int GetNumberOfUpgrades(int listCount)
        {
            var number = Random.Range(0, 2);
            if (number == 0)
                return 0;
            else
                return Random.Range(0, listCount);
        }
    }
}