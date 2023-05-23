using System;
using System.Collections.Generic;
using static ProgressSystem.GameProgressConfig;

namespace DataSaving
{
    [Serializable]
    internal class SavedData
    {
        public int TotalCollectedCoins;
        public List<string> OpenPlayerNames;
        public string CurrentPlayerName;

        public SavedData()
        {
            OpenPlayerNames = new List<string>();
            TotalCollectedCoins = 0;
        }
    }
}