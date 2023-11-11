using GameUI;
using ProgressSystem;

namespace DataSaving
{
    internal class DataController
    {
        private ISelectLogic _selectMenuLogic;
        private readonly IDataSaver _dataSaver;
        private ProgressSavedData _savedData;

        public DataController()
        {
            _dataSaver = new JSONDataSaver();
            LoadProgress();
        }

        public ProgressSavedData SavedData => _savedData;

        public void Init(ISelectLogic selectMenuLogic)
        {
            _selectMenuLogic = selectMenuLogic;
            _selectMenuLogic.UpdatePlayersConfig(SavedData);
        }

        private void LoadProgress() => _savedData = _dataSaver.Load();

        public void SaveProgressFromConfig(GameProgressConfig gameConfig)
        {
            var savedData = new ProgressSavedData()
            {
                TotalCollectedCoins = gameConfig.TotalCoinCount,
                TotalCollectedCrystals = gameConfig.TotalCrystalCount
            };

            for (int i = 0; i < gameConfig.Players.Count; i++)
            {
                if (gameConfig.CurrentPlayer != null 
                    && gameConfig.CurrentPlayer.IsOpen 
                    && gameConfig.CurrentPlayer.Name == gameConfig.Players[i].Name)
                    savedData.CurrentPlayerName = gameConfig.Players[i].Name;

                if (gameConfig.Players[i].IsOpen)
                    savedData.OpenPlayerConfigs.Add(gameConfig.Players[i]);
            }
            _dataSaver.Save(savedData);
        }
    }
}