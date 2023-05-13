using UnityEngine;

namespace Factories
{
    internal class CoinSetView : RespawnableObject
    {
        private void Start()
        {
            _hasChildObjects = true;
        }
    }
}