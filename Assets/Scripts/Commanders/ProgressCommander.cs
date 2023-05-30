using ProgressSystem;
using static ProgressSystem.GameProgressConfig;

namespace Commands
{
    internal class ProgressCommander: ICommander
    {
        private IProgressController _progressController;
        private PlayerConfig _currentPlayer;

        public PlayerConfig CurrentPlayer { get => _currentPlayer; private set => _currentPlayer = value; }
        public GameProgressConfig GameConfig { get => _progressController.GameConfig; }

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