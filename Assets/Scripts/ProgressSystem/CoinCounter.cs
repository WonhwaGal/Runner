using System;

namespace ProgressSystem
{
    internal class CoinCounter
    {
        public Action<int> OnCollectCoins;
        private int _coinsCollected;

        public CoinCounter()
        {
            _coinsCollected = 0;
        }

        public void AddCoins(int value)
        {
            _coinsCollected += value;
            OnCollectCoins?.Invoke(_coinsCollected);
        }

        public int SaveCurrentCoinNumber() => _coinsCollected;
    }
}