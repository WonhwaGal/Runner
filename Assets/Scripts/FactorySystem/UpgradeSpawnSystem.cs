using UnityEngine;

namespace Factories
{
    internal sealed class UpgradeSpawnSystem: IUpgradeSpawnSystem
    {
        private IPool<UpgradeView> _upgradePool;
        private const float MinYShift = 1.0f;
        private const float MaxYShift = 3.0f;

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
            var upgradesNumber = ProbabilityHandler.GetProbability(roadSpan.UpgradeSpots.Count);
            if (upgradesNumber <= 0)
                return;

            for (int i = 0; i < upgradesNumber; i++)
            {
                var YShiftVector = new Vector3(0, Random.Range(MinYShift, MaxYShift), 0);
                UpgradeView upgradeView = _upgradePool.Spawn();
                upgradeView.transform.position = roadSpan.UpgradeSpots[i].position + YShiftVector;
                roadSpan.AcceptChildRespawnable(upgradeView, RespawnableType.Upgrade);
            }
        }
    }
}