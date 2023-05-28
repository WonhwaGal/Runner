using GameUI;
using ProgressSystem;

namespace DataSaving
{
    internal class DataController
    {
        private GameProgressConfig _gameConfig;
        private SelectMenuLogic _selectMenuLogic;
        private IDataSaver _dataSaver;
        private SavedData _savedData;

        public DataController(GameProgressConfig gameConfig)
        {
            _gameConfig = gameConfig;
            _dataSaver = new JSONDataSaver();
        }

        public void Init(SelectMenuLogic selectMenuLogic)
        {
            _selectMenuLogic = selectMenuLogic;
            LoadProgress();
        }
        private void LoadProgress()
        {
            _savedData = _dataSaver.Load();
            _savedData = _selectMenuLogic.UpdatePlayersConfig(_savedData);
            _gameConfig.TotalCoinCount = _savedData.TotalCollectedCoins;
        }

        public void SaveProgress()
        {
            _savedData = new SavedData();
            _savedData.TotalCollectedCoins = _gameConfig.TotalCoinCount;
            foreach(var player in _gameConfig.Players)
            {
                if (player.IsOpen)
                    _savedData.OpenPlayerNames.Add(player.Name);
                if (player.IsCurrent)
                    _savedData.CurrentPlayerName = player.Name;
            }
            _dataSaver.Save(_savedData);
        }
    }
}