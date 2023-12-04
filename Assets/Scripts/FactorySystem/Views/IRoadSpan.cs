using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    internal interface IRoadSpan
    {
        List<Transform> CoinSpots { get; }
        List<Transform> UpgradeSpots { get; }
        void AcceptChildRespawnable(IRespawnable childObject, RespawnableType type);
        void UnparentChildObjects();
    }
}