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
        [SerializeField] private MainCanvas _mainCanvas;


        private IInput _inputType;

        private PlayerControlSystem _playerController;
        private ProgressController _progressController;
        private MainFactory _mainFactory;
        private GameStateColtroller _gameStateController;
        private IUiController _uiController;
        private void Start()
        {
            CreateMainSystems();
            AssignConnections();
        }

        private void Update()
        {
            _inputType.RegisterInput();
        }

        private void CreateMainSystems()
        {
            _inputType = new KeyboardInput();
            _mainFactory = new MainFactory(_firstRoadSpan);

            _uiController = new UIController(_mainCanvas);
            _progressController = new ProgressController(_mainCanvas.GameUIView);
            _playerController = new PlayerControlSystem(_inputType, _mainFactory.RoadSystem);
            _gameStateController = new GameStateColtroller(_playerController, _progressController, _uiController);
        }

        private void AssignConnections()
        {
            _inputType.OnPauseGame += _gameStateController.PauseGame;
            _playerController.OnChoosingPlayer += _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin += _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade += _mainCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle += _gameStateController.StopGame;
        }

        private void SignOffConnections()
        {
            _playerController.OnChoosingPlayer -= _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin -= _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade -= _mainCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle -= _gameStateController.StopGame;
        }

        private void OnDestroy()
        {
            _mainFactory.Dispose();
            _playerController.Dispose();
            _progressController.Dispose();
            _uiController.Dispose();
            _gameStateController.Dispose();
            SignOffConnections();
        }
    }
}