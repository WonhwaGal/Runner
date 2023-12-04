using PlayerSystem;
using ProgressSystem;

namespace Commands
{
    internal class ProgressCommander: ICommander
    {
        private readonly IProgressController _progressController;
        private readonly IPlayerControlSystem _playerControlSystem;

        public ProgressCommander(IProgressController progressController, IPlayerControlSystem playerControlSystem)
        {
            _progressController = progressController;
            _playerControlSystem = playerControlSystem;
        }

        public GameProgressConfig GameConfig => _progressController.GameConfig;

        public void Start() => _playerControlSystem.OnPlayerControllerSet += PlayerCreated;

        public void Pause(bool isPaused) => _progressController.GameUIModel.PauseDistanceCount(isPaused);

        public void Stop()
        {
            _progressController.GameUIModel.StopDistanceCount();
            _playerControlSystem.OnPlayerControllerSet -= PlayerCreated;
            var mover = _playerControlSystem.PlayerController.Mover;
            mover.OnStartRunning -= _progressController.GameUIModel.StartDistanceCount;
            mover.OnSpeedingUp -= _progressController.GameUIModel.IncreaseSpeed;
        }

        public void RegisterCurrentProgress() => _progressController.RegisterCurrentProgress();

        private void PlayerCreated()
        {
            var mover = _playerControlSystem.PlayerController.Mover;
            mover.OnStartRunning += _progressController.GameUIModel.StartDistanceCount;
            mover.OnSpeedingUp += _progressController.GameUIModel.IncreaseSpeed;
        }
    }
}