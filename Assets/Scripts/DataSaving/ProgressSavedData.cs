using System;
using System.Collections.Generic;
using static ProgressSystem.GameProgressConfig;

namespace DataSaving
{
    [Serializable]
    internal class ProgressSavedData
    {
        public int TotalCollectedCoins;
        public int TotalCollectedCrystals;
        public List<PlayerConfig> OpenPlayerConfigs;
        public string CurrentPlayerName;


        public ProgressSavedData()
        {
            OpenPlayerConfigs = new List<PlayerConfig>();
            TotalCollectedCoins = 0;
            TotalCollectedCrystals = 0;
        }
    }
}