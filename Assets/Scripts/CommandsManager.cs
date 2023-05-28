using GameUI;
using PlayerSystem;
using ProgressSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ProgressSystem.GameProgressConfig;

namespace Infrastructure
{
    internal interface ICommander
    {
        void Start();
        void Pause(bool isPaused);
        void Stop();
    }
    internal class CommandsManager
    {
        public Action<bool> OnPause;

        private List<ICommander> _commanders;
        private ICommander _playerCommander;
        private ICommander _uiCommander;
        private ICommander _progressCommander;
        private PauseView _pauseView;
        public ICommander ProgressCommander { get => _progressCommander; private set => _progressCommander = value; }
        public PauseView PauseView { get => _pauseView; private set => _pauseView = value; }
        public CommandsManager(IPlayerControlSystem playerController, IUiController uiController, IProgressController progressController)
        {
            _commanders = new List<ICommander>();
            ProgressCommander = new ProgressCommander(progressController);
            _commanders.Add(ProgressCommander);

            _playerCommander = new PlayerCommander(playerController, progressController.RecieveCurrentPlayer());
            _commanders.Add(_playerCommander);

            _uiCommander = new UICommander(uiController);
            _pauseView = uiController.PauseView;
            _commanders.Add(_uiCommander);
        }
        public void Start()
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Start();
        }
        public void Pause(bool isPaused)
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Pause(isPaused);
            OnPause?.Invoke(isPaused);
        }
        public void Stop()
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Stop();
        }
    }

    internal class PlayerCommander: ICommander
    {
        private IPlayerControlSystem _playerController;
        private PlayerConfig _currentPlayer;
        public PlayerCommander(IPlayerControlSystem playerController, PlayerConfig currentPlayer)
        {
            _playerController = playerController;
            _currentPlayer = currentPlayer;
        }

        public void Start() => _playerController.CreatePlayer(_currentPlayer);
        public void Pause(bool isPaused) => _playerController.PausePlayer(isPaused);
        public void Stop() => _playerController.StopPlayer();
    }
    internal class UICommander : ICommander
    {
        private IUiController _uiController;

        internal IUiController UiController { get => _uiController; private set => _uiController = value; }

        public UICommander(IUiController uiController) => UiController = uiController;

        public void Start() => UiController.PauseView.gameObject.SetActive(false);
        public void Pause(bool isPaused) => UiController.PauseGame(isPaused);
        public void Stop() { }
    }

    internal class ProgressCommander: ICommander
    {
        private IProgressController _progressController;
        private PlayerConfig _currentPlayer;

        public PlayerConfig CurrentPlayer { get => _currentPlayer; private set => _currentPlayer = value; }
        public GameProgressConfig GameConfig { get => _progressController.GameConfig;}

        public ProgressCommander(IProgressController progressController) 
        {
            _progressController = progressController;
            _currentPlayer = _progressController.RecieveCurrentPlayer();
        }
        public void Start() => _progressController.UIModel.StartDistanceCount();
        public void Pause(bool isPaused) => _progressController.UIModel.PauseDistanceCount(isPaused);
        public void Stop() => _progressController.UIModel.StopDistanceCount();
        public void RegisterCurrentProgress() => _progressController.RegisterCurrentProgress();
    }
}