using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IUpgradeSpawnSystem
    {
        void PutUpgradesOnRoad(List<Transform> upgradeSpots);
        void UpdateAnimationState(bool isPaused);
    }
}