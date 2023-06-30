using GameUI;
using PlayerSystem;
using ProgressSystem;
using System;
using System.Collections.Generic;


namespace Commands
{
    internal class CommandsManager
    {
        public Action<bool> OnPause;

        private List<ICommander> _commanders;
        private ICommander _playerCommander;
        private ICommander _uiCommander;
        private ICommander _progressCommander;
        private IPauseView _pauseView;

        public ICommander ProgressCommander { get => _progressCommander; }
        public IPauseView PauseView { get => _pauseView; private set => _pauseView = value; }

        public CommandsManager(IPlayerControlSystem playerControlSystem, IGameUiController uiController, IProgressController progressController)
        {
            _commanders = new List<ICommander>();
            _progressCommander = new ProgressCommander(progressController, playerControlSystem);
            _commanders.Add(_progressCommander);

            _playerCommander = new PlayerCommander(playerControlSystem, progressController.RecieveCurrentPlayer());
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
}