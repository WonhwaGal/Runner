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
        private GameStateColtroller _gameStateColtroller;
        private void Start()
        {
            CreateMainSystems();
            AssignConnections();
        }

        private void Update()
        {
            _inputType.GetXValue();
            _inputType.GetYValue();
        }

        private void CreateMainSystems()
        {
            _inputType = new KeyboardInput();

            _progressController = new ProgressController(_mainCanvas.GameUIView);
            _playerController = new PlayerControlSystem(_inputType);
            _mainFactory = new MainFactory(_firstRoadSpan);
            _gameStateColtroller = new GameStateColtroller(_playerController, _progressController, _mainFactory.RoadSystem);
        }

        private void AssignConnections()
        {
            _playerController.OnChoosingPlayer += _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin += _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade += _mainCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle += _gameStateColtroller.StopGame;
            _mainCanvas.MainMenuView.OnSelectPlayer += _gameStateColtroller.SetCurrentPlayer;
        }

        private void SignOffConnections()
        {
            _playerController.OnChoosingPlayer -= _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin -= _progressController.CoinCounter.AddCoins;
            _playerController.TriggerHandler.OnGettingUpgrade -= _mainCanvas.GameUIView.ActivateUpgradeImage;
            _playerController.TriggerHandler.OnHittingAnObstacle -= _gameStateColtroller.StopGame;
            _mainCanvas.MainMenuView.OnSelectPlayer -= _gameStateColtroller.SetCurrentPlayer;
        }

        private void OnDestroy()
        {
            _mainFactory.Dispose();
            _playerController.Dispose();
            _progressController.Dispose();
            SignOffConnections();
        }
    }
}