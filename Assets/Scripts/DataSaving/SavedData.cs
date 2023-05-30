using System;
using System.Collections.Generic;
using static ProgressSystem.GameProgressConfig;

namespace DataSaving
{
    [Serializable]
    internal class SavedData
    {
        public int TotalCollectedCoins;
        public int TotalCollectedCrystals;
        public List<string> OpenPlayersNames;
        public List<int> TimesLeftToPlay;
        public string CurrentPlayerName;

        public SavedData()
        {
            OpenPlayersNames = new List<string>();
            TimesLeftToPlay = new List<int>();
            TotalCollectedCoins = 0;
            TotalCollectedCrystals = 0;
        }
    }
}