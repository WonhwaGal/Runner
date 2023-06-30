using PlayerSystem;
using ProgressSystem;

namespace Commands
{
    internal class ProgressCommander: ICommander
    {
        private IProgressController _progressController;
        private IPlayerControlSystem _playerControlSystem;

        public GameProgressConfig GameConfig { get => _progressController.GameConfig; }

        public ProgressCommander(IProgressController progressController, IPlayerControlSystem playerControlSystem)
        {
            _progressController = progressController;
            _playerControlSystem = playerControlSystem;
        }


        public void Start() => _playerControlSystem.OnPlayerControllerSet += PlayerCreated;

        public void Pause(bool isPaused) => _progressController.GameUIModel.PauseDistanceCount(isPaused);
        public void Stop()
        {
            _progressController.GameUIModel.StopDistanceCount();
            _playerControlSystem.OnPlayerControllerSet -= RegisterCurrentProgress;
            _playerControlSystem.PlayerController.Mover.OnStartRunning -= _progressController.GameUIModel.StartDistanceCount;
            _playerControlSystem.PlayerController.Mover.OnSpeedingUp -= _progressController.GameUIModel.IncreaseSpeed;
        }

        public void RegisterCurrentProgress() => _progressController.RegisterCurrentProgress();

        private void PlayerCreated()
        {
            _playerControlSystem.PlayerController.Mover.OnStartRunning += _progressController.GameUIModel.StartDistanceCount;
            _playerControlSystem.PlayerController.Mover.OnSpeedingUp += _progressController.GameUIModel.IncreaseSpeed;
        }
    }
}