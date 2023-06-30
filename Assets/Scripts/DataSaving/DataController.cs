using GameUI;
using ProgressSystem;
using UnityEngine;

namespace DataSaving
{
    internal class DataController
    {
        private ISelectLogic _selectMenuLogic;
        private IDataSaver _dataSaver;
        private SavedData _savedData;

        internal SavedData SavedData { get => _savedData; }

        public DataController()
        {
            _dataSaver = new JSONDataSaver();
            LoadProgress();
        }

        public void Init(ISelectLogic selectMenuLogic)
        {
            _selectMenuLogic = selectMenuLogic;
            _selectMenuLogic.UpdatePlayersConfig(SavedData);
        }

        private void LoadProgress() => _savedData = _dataSaver.Load();

        public void SaveProgressFromConfig(GameProgressConfig gameConfig)
        {
            var savedData = new SavedData();
            savedData.TotalCollectedCoins = gameConfig.TotalCoinCount;
            savedData.TotalCollectedCrystals = gameConfig.TotalCrystalCount;
            for (int i = 0; i < gameConfig.Players.Count; i++)
            {
                if (gameConfig.Players[i].IsCurrent && gameConfig.Players[i].IsOpen)
                    savedData.CurrentPlayerName = gameConfig.Players[i].Name;
                if (gameConfig.Players[i].IsOpen)
                    savedData.OpenPlayerConfigs.Add(gameConfig.Players[i]);
            }
            _dataSaver.Save(savedData);
        }
    }
}