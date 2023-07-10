using DataSaving;
using ProgressSystem;
using System;
using static ProgressSystem.GameProgressConfig;

namespace GameUI
{
    internal class SelectMenuLogic: ISelectLogic
    {
        public event Action OnPlayerSelected;
        public event Action<int> OnSettingCoinNumber;
        public event Action<int> OnSettingCrystalNumber;
        public event Action<GameProgressConfig> OnChangingGameCfg;

        private GameProgressConfig _gameConfig;

        public void AssignPlayerConfig(GameProgressConfig gameConfig) => _gameConfig = gameConfig;

        public void UpdatePlayersConfig(SavedData savedData)
        {
            if (savedData.OpenPlayerConfigs.Count == 0)
                AddDefaultPlayer();
            else
                DrawDataFromSavedData(savedData);
        }

        private void AddDefaultPlayer()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                if (_gameConfig.Players[i].IsDefault)
                    _gameConfig.Players[i].IsOpen = true;

                ConfigCollectablesToZero();
            }
            UpdateGameUIFromConfig();
            OnChangingGameCfg?.Invoke(_gameConfig);
        }

        private void DrawDataFromSavedData(SavedData savedData)
        {
            UnityEngine.Debug.Log("drawing from savedData");
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _gameConfig.Players[i].IsOpen = false;
                for (int s = 0; s < savedData.OpenPlayerConfigs.Count; s++)
                {
                    if (savedData.OpenPlayerConfigs[s].Name == _gameConfig.Players[i].Name)
                        _gameConfig.Players[i].IsOpen = true;
                }

                if (savedData.CurrentPlayerName != string.Empty
                    && savedData.CurrentPlayerName == _gameConfig.Players[i].Name)
                {
                    _gameConfig.CurrentPlayer = _gameConfig.Players[i];
                    _gameConfig.Players[i].IsCurrent = true;
                }
                else if (savedData.CurrentPlayerName != _gameConfig.Players[i].Name)
                    _gameConfig.Players[i].IsCurrent = false;
            }
            _gameConfig.TotalCoinCount = savedData.TotalCollectedCoins;
            _gameConfig.TotalCrystalCount = savedData.TotalCollectedCrystals;
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
                {
                    _gameConfig.CurrentPlayer = _gameConfig.Players[i];
                    _gameConfig.Players[i].IsCurrent = true;
                }
                else
                    _gameConfig.Players[i].IsCurrent = false;
            }
            OnChangingGameCfg?.Invoke(_gameConfig);
            OnPlayerSelected?.Invoke();
        }

        public void BuyPlayer(PlayerConfig config)
        {
            config.IsOpen = true;
            if (config.CurrencyType == CurrencyType.Coins)
                _gameConfig.TotalCoinCount -= config.CurrencyPrice;
            else
                _gameConfig.TotalCrystalCount -= config.CurrencyPrice;

            OnSettingCoinNumber?.Invoke(_gameConfig.TotalCoinCount);
            OnSettingCrystalNumber?.Invoke(_gameConfig.TotalCrystalCount);
            OnChangingGameCfg?.Invoke(_gameConfig);
        }

        public void CancelProgress()
        {
            for (int i = 0; i < _gameConfig.Players.Count; i++)
            {
                _gameConfig.Players[i].IsCurrent = false;
                if (_gameConfig.Players[i].IsDefault)
                {
                    _gameConfig.Players[i].IsOpen = true;
                    _gameConfig.Players[i].IsCurrent = true;
                    _gameConfig.CurrentPlayer = _gameConfig.Players[i];
                }
                else
                    _gameConfig.Players[i].IsOpen = false;

                _gameConfig.Players[i].TimesLeftToPlay = _gameConfig.Players[i].TimesToPlay;
            }
            ConfigCollectablesToZero();
            UpdateGameUIFromConfig();
            OnChangingGameCfg?.Invoke(_gameConfig);
        }

        private void ConfigCollectablesToZero()
        {
            _gameConfig.TotalCoinCount = 0;
            _gameConfig.TotalCrystalCount = 0;
        }
    }
}