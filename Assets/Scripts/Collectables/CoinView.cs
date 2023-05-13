using UnityEngine;

namespace Collectables
{
    internal class CoinView: MonoBehaviour, ICollectable
    {
        public void ExecuteAction()
        {
            gameObject.SetActive(false);
        }
    }
}