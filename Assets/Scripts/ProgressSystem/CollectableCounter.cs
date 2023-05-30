using System;

namespace ProgressSystem
{
    internal class CollectableCounter
    {
        public Action<int> OnCollectCoins { get; set; }
        public Action<int> OnCollectCrystals { get; set; }
        private int _coinsCollected;
        private int _crystalsCollected;

        public void AddCoins(int value)
        {
            _coinsCollected += value;
            OnCollectCoins?.Invoke(_coinsCollected);
        }
        public void AddCrystals()
        {
            _crystalsCollected++;
            OnCollectCrystals?.Invoke(_crystalsCollected);
        }
        public void SetCoinNumber(int coins, int crystals)
        {
            _coinsCollected = coins;
            OnCollectCoins?.Invoke(_coinsCollected);
            _crystalsCollected = crystals;
            OnCollectCrystals?.Invoke(_crystalsCollected);
        }
        public int SaveCurrentCoinNumber() => _coinsCollected;
        public int SaveCurrentCrystalNumber() => _crystalsCollected;
    }
}