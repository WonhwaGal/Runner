using UnityEngine;
using System.Collections.Generic;
using Collectables;

namespace Factories
{
    internal class RoadSpan : RespawnableObject
    {
        [SerializeField] private List<Transform> _spots;

        public List<Transform> Spots { get => _spots; }
    }
}