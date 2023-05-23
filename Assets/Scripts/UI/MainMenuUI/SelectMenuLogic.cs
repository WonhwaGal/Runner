using DataSaving;
using ProgressSystem;
using System;
using static ProgressSystem.GameProgressConfig;

namespace GameUI
{
    internal class SelectMenuLogic
    {
        public Action OnPlayerSelected { get; set; }
        public Action<int> OnSettingCoinNumber { get; set; }

        private SavedData _savedData;
        private GameProgressConfig _gameConfig;

        public void AssignPlayerConfig(GameProgressConfig playerTypes) => _gameConfig = playerTypes;
        public void SelectChosenPlayer(PlayerConfig config)
        {
            config.IsCurrent = true;
            FindCurrentPlayer();
        }
        public void BuyPlayer(PlayerConfig config)
        {
            config.IsOpen = true;
            _savedData.OpenPlayerNames.Add(config.Name);
        }
        public SavedData UpdatePlayersConfig(SavedData savedData)
        {
            _savedData = savedData;
            if (_savedData.OpenPlayerNames.Count == 0)
                return DrawDataFromConfig();
            else
                return DrawDataFromSavedData();
        }
        public void CancelProgress()
        {
            for(int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _gameConfig.Players[i].IsCurrent = false;
                if (_gameConfig.Players[i].IsDefault == false)
                    _gameConfig.Players[i].IsOpen = false;
                else
                    _gameConfig.Players[i].IsOpen = true;
            }
            _savedData.OpenPlayerNames.Clear();
            _savedData.TotalCollectedCoins = 0;
            _gameConfig.TotalCoinCount = 0;
            UpdatePlayersConfig(_savedData);
        }
        public void SelectCurrentPlayer()
        {
            bool currentPlayerFound = FindCurrentPlayer();
            if (!currentPlayerFound)
                SelectDefaultPlayer();
        }

        private bool FindCurrentPlayer()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].IsCurrent)
                {
                    OnPlayerSelected?.Invoke();
                    return true;
                }
            }
            return false;
        }
        private void SelectDefaultPlayer()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].IsDefault)
                {
                    _gameConfig.Players[i].IsCurrent = true;
                    OnPlayerSelected?.Invoke();
                    return;
                }
                else
                    _gameConfig.Players[i].IsCurrent = false;
            }
        }
        private SavedData DrawDataFromConfig()
        {
            foreach (var player in _gameConfig.Players)
            {
                if (player.IsDefault)
                {
                    _savedData.OpenPlayerNames.Add(player.Name);
                    player.IsOpen = true;
                }
            }
            OnSettingCoinNumber?.Invoke(0);
            return _savedData;
        }
        private SavedData DrawDataFromSavedData()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_savedData.OpenPlayerNames.Contains(_gameConfig.Players[i].Name))
                    _gameConfig.Players[i].IsOpen = true;
                else
                    _gameConfig.Players[i].IsOpen = false;

                if (_savedData.CurrentPlayerName != string.Empty
                    && _savedData.CurrentPlayerName == _gameConfig.Players[i].Name)
                    _gameConfig.Players[i].IsCurrent = true;
                else if (_savedData.CurrentPlayerName != _gameConfig.Players[i].Name)
                    _gameConfig.Players[i].IsCurrent = false;
            }

            OnSettingCoinNumber?.Invoke(_savedData.TotalCollectedCoins);
            _gameConfig.TotalCoinCount = _savedData.TotalCollectedCoins;
            UnityEngine.Debug.Log("total from saved data = "+ _gameConfig.TotalCoinCount);
            return _savedData;
        }
    }
}