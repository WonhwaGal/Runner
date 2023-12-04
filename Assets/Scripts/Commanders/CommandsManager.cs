using System.Collections.Generic;
using GameUI;
using PlayerSystem;
using ProgressSystem;

namespace Commands
{
    internal sealed class CommandsManager
    {
        private readonly List<ICommander> _commanders;
        private readonly ICommander _playerCommander;
        private readonly ICommander _uiCommander;
        private readonly ICommander _progressCommander;
        private IPauseView _pauseView;

        public CommandsManager(IPlayerControlSystem playerControlSystem,
                        GameUIController uiController, IProgressController progressController)
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

        public ICommander ProgressCommander => _progressCommander;
        public IPauseView PauseView { get => _pauseView; private set => _pauseView = value; }

        public void Start()
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Start();
        }

        public void Pause(bool isPaused)
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Pause(isPaused);
        }

        public void Stop()
        {
            for (int i = 0; i < _commanders.Count; i++)
                _commanders[i].Stop();
        }
    }
}