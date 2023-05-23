using Factories;
using GameUI;
using PlayerSystem;
using ProgressSystem;
using UnityEngine;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _firstRoadSpan;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private GameCanvas _gameCanvas;
        [SerializeField] private GameProgressConfig _gameConfig;

        private IInput _inputType;

        private IPlayerControlSystem _playerController;
        private IProgressController _progressController;
        private MainFactory _mainFactory;
        private GameStateColtroller _gameStateController;
        private IUiController _uiController;

        private void Start()
        {
            CreateMainSystems();
            AssignConnections();
        }

        private void Update() => _inputType.RegisterInput();

        private void CreateMainSystems()
        {
            _inputType = new KeyboardInput();
            _mainFactory = new MainFactory(_firstRoadSpan);
            _uiController = new GameUIController(_gameCanvas);
            _progressController = new ProgressController(_gameCanvas.GameUIView, _gameConfig);
            _playerController = new PlayerControlSystem(_inputType, _mainFactory.RoadSystem, _cameraFollow);
            _gameStateController = new GameStateColtroller(_playerController, _progressController, _uiController);
        }

        private void AssignConnections()
        {
            _inputType.OnPauseGame += _gameStateController.PauseGame;
            _playerController.TriggerHandler.OnTriggeredByCoin += _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade += _gameCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle += _gameStateController.LoseGame;
        }

        private void SignOffConnections()
        {
            _inputType.OnPauseGame -= _gameStateController.PauseGame;
            _playerController.TriggerHandler.OnTriggeredByCoin -= _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade -= _gameCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle -= _gameStateController.LoseGame;
        }

        private void OnDestroy()
        {
            _mainFactory.Dispose();
            _playerController.Dispose();
            _progressController.Dispose();
            _gameStateController.Dispose();
            SignOffConnections();
        }
    }
}