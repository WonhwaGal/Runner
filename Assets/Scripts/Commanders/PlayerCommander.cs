using PlayerSystem;
using static ProgressSystem.GameProgressConfig;

namespace Commands
{
    internal class PlayerCommander: ICommander
    {
        private readonly IPlayerControlSystem _playerController;
        private readonly PlayerConfig _currentPlayer;

        public PlayerCommander(IPlayerControlSystem playerController, PlayerConfig currentPlayer)
        {
            _playerController = playerController;
            _currentPlayer = currentPlayer;
        }


        public void Start() => _playerController.CreatePlayer(_currentPlayer);

        public void Pause(bool isPaused) => _playerController.PausePlayer(isPaused);

        public void Stop() => _playerController.StopPlayer();
    }
}