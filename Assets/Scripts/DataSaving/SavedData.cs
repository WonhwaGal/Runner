using System;
using System.Collections.Generic;
using static ProgressSystem.PlayerCfgList;

namespace DataSaving
{
    [Serializable]
    internal class SavedData
    {
        public int TotalCollectedCoins;
        public Dictionary<string, PlayerConfig> OpenPlayers;
        public PlayerConfig CurrentPlayer;

        public SavedData()
        {
            OpenPlayers = new Dictionary<string, PlayerConfig>();
            TotalCollectedCoins = 0;
        }
    }
}