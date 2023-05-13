using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    internal class RoadSpan : RespawnableObject
    {
        [SerializeField] private List<Transform> _spots;

        public List<Transform> Spots { get => _spots; }
    }
}