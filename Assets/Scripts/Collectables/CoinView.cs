using UnityEngine;

namespace Collectables
{
    internal class CoinView: MonoBehaviour, ICollectable
    {
        private int _value;
        public void ExecuteAction()
        {
            gameObject.SetActive(false);
        }
    }
}