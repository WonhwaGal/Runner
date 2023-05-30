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
        public Action<int> OnSettingCrystalNumber { get; set; }
        public Action<SavedData> OnChangingGameCfg { get; set; }

        private SavedData _savedData = new();
        private GameProgressConfig _gameConfig;

        public void AssignPlayerConfig(GameProgressConfig gameConfig) => _gameConfig = gameConfig;
        public void UpdatePlayersConfig(SavedData savedData)
        {
            _savedData = savedData;
            if (_savedData.OpenPlayersNames.Count == 0)
                AddDefaultPlayer();
            else
                DrawDataFromSavedData();
        }
        private void AddDefaultPlayer()
        {
            UnityEngine.Debug.Log("Adding default player");
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _savedData.TimesLeftToPlay.Add(_gameConfig.Players[i].TimesLeftToPlay);
                if (_gameConfig.Players[i].IsDefault)
                {
                    _gameConfig.Players[i].IsOpen = true;
                    _savedData.CurrentPlayerName = _gameConfig.Players[i].Name;
                    _savedData.OpenPlayersNames.Add(_gameConfig.Players[i].Name);
                }
                ConfigCollectablesToZero();
            }
            UpdateGameUIFromConfig();
            OnChangingGameCfg?.Invoke(_savedData);
        }
        private void DrawDataFromSavedData()
        {
            UnityEngine.Debug.Log("drawing from savedData");
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _gameConfig.Players[i].TimesLeftToPlay = _savedData.TimesLeftToPlay[i];

                if (_savedData.OpenPlayersNames.Contains(_gameConfig.Players[i].Name))
                    _gameConfig.Players[i].IsOpen = true;
                else
                    _gameConfig.Players[i].IsOpen = false;

                if (_savedData.CurrentPlayerName != string.Empty
                    && _savedData.CurrentPlayerName == _gameConfig.Players[i].Name)
                    _gameConfig.Players[i].IsCurrent = true;
                else if (_savedData.CurrentPlayerName != _gameConfig.Players[i].Name)
                    _gameConfig.Players[i].IsCurrent = false;
            }
            _gameConfig.TotalCoinCount = _savedData.TotalCollectedCoins;
            _gameConfig.TotalCrystalCount = _savedData.TotalCollectedCrystals;
            UpdateGameUIFromConfig();
        }
        private void UpdateGameUIFromConfig()
        {
            OnSettingCoinNumber?.Invoke(_gameConfig.TotalCoinCount);
            OnSettingCrystalNumber?.Invoke(_gameConfig.TotalCrystalCount);
        }
        public void ChangeCurrentPlayerTo(PlayerConfig config)
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].Name == config.Name)
                    _gameConfig.Players[i].IsCurrent = true;
                else
                    _gameConfig.Players[i].IsCurrent = false;
            }
            _savedData.CurrentPlayerName = config.Name;
            OnChangingGameCfg?.Invoke(_savedData);
            OnPlayerSelected?.Invoke();
        }
        public void BuyPlayer(PlayerConfig config)
        {
            config.IsOpen = true;
            _gameConfig.TotalCoinCount -= config.CoinPrice;

            _savedData.OpenPlayersNames.Add(config.Name);
            _savedData.TotalCollectedCoins = _gameConfig.TotalCoinCount;
            _savedData.TotalCollectedCrystals = _gameConfig.TotalCrystalCount;

            OnSettingCoinNumber?.Invoke(_gameConfig.TotalCoinCount);
            OnSettingCrystalNumber?.Invoke(_gameConfig.TotalCrystalCount);
            OnChangingGameCfg?.Invoke(_savedData);
        }
        public void CancelProgress()
        {
            _savedData.OpenPlayersNames.Clear();
            _savedData.TimesLeftToPlay.Clear();

            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _gameConfig.Players[i].IsCurrent = false;
                if (_gameConfig.Players[i].IsDefault)
                {
                    _gameConfig.Players[i].IsOpen = true;
                    _savedData.OpenPlayersNames.Add(_gameConfig.Players[i].Name);
                }
                else
                    _gameConfig.Players[i].IsOpen = false;
                _gameConfig.Players[i].TimesLeftToPlay = _gameConfig.Players[i].TimesToPlay;
                _savedData.TimesLeftToPlay.Add(_gameConfig.Players[i].TimesToPlay);
            }
            ConfigCollectablesToZero();
            UpdateGameUIFromConfig();
            _savedData.TotalCollectedCoins = 0;
            _savedData.TotalCollectedCrystals = 0;
            OnChangingGameCfg?.Invoke(_savedData);
        }
        // When Start pressed without choosing
        public void SelectCurrentPlayer()
        {
            var currentPlayer = FindCurrentPlayer();
            if (currentPlayer == null || !currentPlayer.IsOpen)
                SelectDefaultPlayer();
            else if (currentPlayer != null)
            {
                UnityEngine.Debug.Log("chose new current player");
                _savedData.CurrentPlayerName = currentPlayer.Name;
                OnChangingGameCfg?.Invoke(_savedData);
                OnPlayerSelected?.Invoke();
            }
        }
        private PlayerConfig FindCurrentPlayer()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].IsCurrent)
                    return _gameConfig.Players[i];
            }
            return null;
        }
        private void SelectDefaultPlayer()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].IsDefault)
                {
                    _gameConfig.Players[i].IsCurrent = true;
                    _gameConfig.Players[i].IsOpen = true;
                    OnPlayerSelected?.Invoke();
                    return;
                }
            }
        }
        private void ConfigCollectablesToZero()
        {
            _gameConfig.TotalCoinCount = 0;
            _gameConfig.TotalCrystalCount = 0;
        }
    }
}