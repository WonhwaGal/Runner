using UnityEngine;
using System.Collections.Generic;
using Collectables;

namespace Factories
{
    internal class RoadSpan : RespawnableObject
    {
        [SerializeField] private List<Transform> _coinSpots;
        [SerializeField] private List<Transform> _upgradeSpots;

        public List<Transform> Spots { get => _coinSpots; }
        public List<Transform> UpgradeSpots { get => _upgradeSpots; }
    }
}