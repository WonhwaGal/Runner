using UnityEngine;
using GameUI;
using Commands;
using Factories;
using PlayerSystem;
using ProgressSystem;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _firstRoadSpan;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private GameCanvas _gameCanvas;
        [SerializeField] private GameProgressConfig _gameConfig;

        private IInput _inputType;

        private IPlayerControlSystem _playerControlSystem;
        private IProgressController _progressController;
        private MainFactory _mainFactory;
        private GameStateColtroller _gameStateController;
        private GameUIController _uiController;
        private CommandsManager _commandsManager;

        private void Start() => CreateMainSystems();

        private void Update() => _inputType.RegisterInput();

        private void CreateMainSystems()
        {
            _inputType = new KeyboardInput();
            _mainFactory = new MainFactory(_firstRoadSpan);
            _uiController = new GameUIController(_gameCanvas);
            _progressController = new ProgressController(_gameCanvas.GameUIView, _gameConfig);
            _playerControlSystem = new PlayerControlSystem(_inputType, _mainFactory.RoadSystem, _cameraFollow);
            _commandsManager = new CommandsManager(_playerControlSystem, _uiController, _progressController);
            _gameStateController = new GameStateColtroller(_commandsManager);
        }

        private void OnDestroy()
        {
            _mainFactory.Dispose();
            _playerControlSystem.Dispose();
            _progressController.Dispose();
            _gameStateController.Dispose();
            GameEventSystem.Dispose();
        }
    }
}