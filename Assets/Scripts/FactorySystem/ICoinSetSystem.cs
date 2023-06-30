using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface ICoinSetSystem
    {
        void PutCoinsOnRoad(List<Transform> coinSetSpots);
        void UpdateAnimationState(bool isPaused);
    }
}