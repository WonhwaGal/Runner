using System;

namespace ProgressSystem
{
    internal interface ICollectableCounter
    {
        Action<int> OnCollectCoins { get; set; }
        Action<int> OnCollectCrystals { get; set; }

        void AddCoins(int value);
        void AddCrystals(int value);

        void SetCoinNumber(int coins, int crystals);
        int SetCurrentCoinNumber();
        int SetCurrentCrystalNumber();
    }
}