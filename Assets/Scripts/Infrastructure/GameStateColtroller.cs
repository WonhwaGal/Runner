using DataSaving;
using GameUI;
using PlayerSystem;
using ProgressSystem;
using System;
using static ProgressSystem.PlayerCfgList;


namespace Infrastructure
{
    internal class GameStateColtroller
    {
        public Action OnGameStop;

        private PlayerControlSystem _playerController;
        private ProgressController _progressController;
        private PlayerConfig _currentPlayer;
        private IUiController _uiController;
        private SelectMenuLogic _selectMenuLogic;

        private IDataSaver _dataSaver;
        private SavedData _savedData;

        private bool _isPaused;

        public GameStateColtroller(PlayerControlSystem playerController,
            ProgressController progressController,
            IUiController uiController)
        {
            _playerController = playerController;
            _progressController = progressController;
            _uiController = uiController;
            _selectMenuLogic = _uiController.SelectLogic;
            _selectMenuLogic.OnSelectPlayer += SetCurrentPlayer;
            _selectMenuLogic.OnCancellingProgress += _progressController.CoinCounter.SetCoinNumber;
            _selectMenuLogic.OnBuyingPlayer += _progressController.SpendCoinsOnPlayer;
            _dataSaver = new JSONDataSaver();
            LoadProgress();
        }
        public void SetCurrentPlayer(PlayerConfig currentPlayer)
        {
            _currentPlayer = currentPlayer;
            currentPlayer.IsCurrent = true;
            StartGame();
        }
        public void StartGame()
        {
            _playerController.CreatePlayer(_currentPlayer);
            _progressController.UIModel.StartDistanceCount();
        }
        public void PauseGame() 
        {
            _isPaused = !_isPaused;
            _uiController.PauseGame(_isPaused);
            _playerController.PausePlayer(_isPaused);
        }

        public void StopGame()
        {
            _playerController.StopPlayer();
            _savedData = _selectMenuLogic.UpdateAfterGame();
            _savedData.TotalCollectedCoins = _progressController.GetCurrentProgress();
            _dataSaver.Save(_savedData);
        }
        private void LoadProgress()
        {
            _savedData = _dataSaver.Load();
            _savedData = _selectMenuLogic.UpdatePlayersConfig(_savedData);

            if (_savedData.CurrentPlayer != null)
                _currentPlayer = _savedData.CurrentPlayer;

            _progressController.CoinCounter.AddCoins(_savedData.TotalCollectedCoins);
        }

        public void Dispose()
        {
            _selectMenuLogic.OnSelectPlayer -= SetCurrentPlayer;
            _selectMenuLogic.OnCancellingProgress -= _progressController.CoinCounter.SetCoinNumber;
            _selectMenuLogic.OnBuyingPlayer -= _progressController.SpendCoinsOnPlayer;
        }
    }
}