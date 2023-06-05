using GameUI;
using ProgressSystem;
using UnityEngine;

namespace DataSaving
{
    internal class DataController
    {
        private SelectMenuLogic _selectMenuLogic;
        private IDataSaver _dataSaver;
        private SavedData _savedData;

        internal SavedData SavedData { get => _savedData; }

        public DataController()
        {
            //UnityEngine.Debug.Log(Application.persistentDataPath + "/DataSaver.json");
            _dataSaver = new JSONDataSaver();
            LoadProgress();
        }

        public void Init(SelectMenuLogic selectMenuLogic)
        {
            _selectMenuLogic = selectMenuLogic;
            _selectMenuLogic.UpdatePlayersConfig(SavedData);
        }

        private void LoadProgress() => _savedData = _dataSaver.Load();

        public void SaveProgress(SavedData savedData) => _dataSaver.Save(savedData);

        public void SaveProgressFromConfig(GameProgressConfig gameConfig)
        {
            var savedData = new SavedData();
            savedData.TotalCollectedCoins = gameConfig.TotalCoinCount;
            savedData.TotalCollectedCrystals = gameConfig.TotalCrystalCount;
            for (int i = 0; i < gameConfig.Players.Count; i++)
            {
                savedData.TimesLeftToPlay.Add(gameConfig.Players[i].TimesLeftToPlay);
                if (gameConfig.Players[i].IsCurrent && gameConfig.Players[i].IsOpen)
                    savedData.CurrentPlayerName = gameConfig.Players[i].Name;
                if (gameConfig.Players[i].IsOpen)
                    savedData.OpenPlayersNames.Add(gameConfig.Players[i].Name);
            }
            _dataSaver.Save(savedData);
        }
    }
}