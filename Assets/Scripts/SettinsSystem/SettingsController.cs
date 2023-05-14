using SettingsSystem;
using System;
using System.Collections.Generic;

namespace Infrastructure
{
    internal class SettingsController
    {
        // private UIView _uiView;
        private Model _uiModel;
        // private UIPresenter _uiPresenter;
        // private DataContainer _dataContainer;
        private PlayerConfig _currentPlayer;

        private List<PlayerConfig> _players;

        public PlayerConfig CurrentPlayer 
        { 
            get => _currentPlayer;
            set
            {
                UnityEngine.Debug.Log("came to setter");
                if (_currentPlayer != value)
                {
                    _currentPlayer = value;
                    OnChangingPlayer?.Invoke(_currentPlayer);
                    UnityEngine.Debug.Log("player changed, event sent");
                }
            } 
        }
        public Model UiModel { get => _uiModel; set => _uiModel = value; }  // to delete

        public event Action<PlayerConfig> OnChangingPlayer;

        public SettingsController(PlayerCfgList playerTypes)
        {
            _players = playerTypes.Players;
            UiModel = new Model(playerTypes, _currentPlayer);
        }
    }
    internal class Model
    {
        private List<PlayerConfig> _players;
        private PlayerConfig _currentPlayer;
        public Model(PlayerCfgList playerTypes, PlayerConfig currentPlayer)
        {
            _players = playerTypes.Players;
            _currentPlayer = currentPlayer;
        }

        public void AssignPlayer(string name)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].Name == name)
                {
                    _currentPlayer = _players[i];
                }
            }
        }
    }
}