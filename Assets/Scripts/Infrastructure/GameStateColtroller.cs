using Factories;
using PlayerSystem;
using ProgressSystem;
using static ProgressSystem.PlayerCfgList;

namespace Infrastructure
{
    internal class GameStateColtroller
    {
        private PlayerControlSystem _playerController;
        private ProgressController _progressController;
        private RoadSystem _roadSystem;
        private PlayerConfig _currentPlayer;

        public GameStateColtroller(PlayerControlSystem playerController,
            ProgressController progressController,
            RoadSystem roadSystem)
        {
            _playerController = playerController;
            _progressController = progressController;
            _roadSystem = roadSystem;
        }
        public void SetCurrentPlayer(PlayerConfig currentPlayer)
        {
            _currentPlayer = currentPlayer;
            StartGame();
        }
        public void StartGame()
        {
            _playerController.CreatePlayer(_currentPlayer);
            _progressController.UIModel.StartDistanceCount();
            _roadSystem.StartRoadSpawn();
        }
        public void PauseGame() { }

        public void StopGame()
        {
            _playerController.StopPlayer();
            _progressController.AddCurrentProgress();
            _roadSystem.StopRoadSpawn();
        }
    }
}