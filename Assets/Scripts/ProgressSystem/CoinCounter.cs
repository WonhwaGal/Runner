using System;

namespace ProgressSystem
{
    internal class CoinCounter
    {
        public Action<int> OnCollectCoins;
        private int _coinsCollected;

        public void AddCoins(int value)
        {
            _coinsCollected += value;
            OnCollectCoins?.Invoke(_coinsCollected);
        }

        public void SetCoinNumber(int value)
        {
            _coinsCollected = value;
            OnCollectCoins?.Invoke(_coinsCollected);
        }
        public int SaveCurrentCoinNumber() => _coinsCollected;
    }
}