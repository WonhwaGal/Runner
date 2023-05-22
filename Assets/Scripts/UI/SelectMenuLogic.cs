using DataSaving;
using ProgressSystem;
using System;
using static ProgressSystem.PlayerCfgList;

namespace GameUI
{
    internal class SelectMenuLogic
    {
        public Action<PlayerConfig> OnSelectPlayer { get; set; }
        public Action<PlayerConfig> OnBuyingPlayer { get; set; }
        public Action<int> OnSettingCoinNumber { get; set; }
        public Action<int> OnCancellingProgress { get; set; }

        private SavedData _savedData;
        private PlayerCfgList _playerTypes;

        public void AssignPlayerConfig(PlayerCfgList playerTypes) => _playerTypes = playerTypes;
        public void SelectChosenPlayer(PlayerConfig config) => OnSelectPlayer?.Invoke(config);
        public void BuyPlayer(PlayerConfig config) => OnBuyingPlayer?.Invoke(config);
        public void AddNewPlayer(PlayerConfig config) => _savedData.OpenPlayers.Add(config.Name, config);
        public SavedData UpdatePlayersConfig(SavedData savedData)
        {
            _savedData = savedData;
            if (_savedData.OpenPlayers.Count == 0)
            {
                foreach (var player in _playerTypes.Players)
                {
                    if (player.IsDefault)
                    {
                        _savedData.OpenPlayers.Add(player.Name, player);
                        player.IsOpen = true;
                    }
                }
            }
            for(int i = 0; i < _playerTypes.Players.Count; i++)
            {
                if (_savedData.OpenPlayers.ContainsKey(_playerTypes.Players[i].Name))
                    _playerTypes.Players[i].IsOpen = true;
                else
                    _playerTypes.Players[i].IsOpen = false;

                if (_savedData.CurrentPlayer != null && _savedData.CurrentPlayer == _playerTypes.Players[i])
                    _playerTypes.Players[i].IsCurrent = true;
            }
            OnSettingCoinNumber?.Invoke(_savedData.TotalCollectedCoins);

            return _savedData;
        }

        public SavedData UpdateAfterGame() => _savedData;

        public void CancelProgress()
        {
            for(int i = 0; i < _playerTypes.Players.Count; i++)
            {
                if (_playerTypes.Players[i].IsDefault == false)
                {
                    _playerTypes.Players[i].IsCurrent = false;
                    _playerTypes.Players[i].IsOpen = false;
                }
                else
                    _playerTypes.Players[i].IsOpen = true;
            }
            _savedData.OpenPlayers.Clear();
            _savedData.TotalCollectedCoins = 0;
            OnCancellingProgress?.Invoke(_savedData.TotalCollectedCoins);
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
            for (int i = 0; i < _playerTypes.Players.Count; i++)
            {
                if (_playerTypes.Players[i].IsCurrent)
                {
                    OnSelectPlayer?.Invoke(_playerTypes.Players[i]);
                    return true;
                }
            }
            return false;
        }

        private void SelectDefaultPlayer()
        {
            for (int i = 0; i < _playerTypes.Players.Count; i++)
            {
                if (_playerTypes.Players[i].IsDefault)
                {
                    OnSelectPlayer?.Invoke(_playerTypes.Players[i]);
                    return;
                }
            }
        }
    }
}