using UnityEngine;
using DG.Tweening;


namespace Collectables
{
    internal class CoinView: MonoBehaviour, ICollectable
    {
        private int _value;
        public int Value { get => _value; set => _value = value; }

        public CollectableType Type { get; private set; }

        private void Start()
        {
            _value = 1;
            Type = CollectableType.Coin;
        }
        public void ExecuteAction()
        {
            gameObject.SetActive(false);
        }
    }
}