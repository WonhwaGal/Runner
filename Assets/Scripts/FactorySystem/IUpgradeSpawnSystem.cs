using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IUpgradeSpawnSystem
    {
        void PutUpgradesOnRoad(IRoadSpan roadSpan);
        void UpdateAnimationState(bool isPaused);
    }
}