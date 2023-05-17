using Factories;
using PlayerSystem;
using SettingsSystem;
using Tools;
using UnityEngine;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _firstRoadSpan;
        [SerializeField] private CameraFollow _cameraFollow;
        [SerializeField] private UIView _uiView;
        [SerializeField] private PlayerCfgList _playerTypes;

        private IInput _inputType;

        private PlayerControlSystem _playerController;
        private SettingsController _settingsController;
        private MainFactory _mainFactory;
        
        private void Start()
        {
            CreateMainSystems();
            AssignConnections();
        }

        private void Update()
        {
            _inputType.GetXValue();
            _inputType.GetYValue();

            if (Input.GetKeyDown(KeyCode.S))
            {
                _playerController.CreatePlayer(_playerTypes.Players[0]);
                _settingsController.UIModel.StartDistanceCount();
            }
        }

        private void CreateMainSystems()
        {
            _inputType = new KeyboardInput();

            _settingsController = new SettingsController(_uiView);
            _playerController = new PlayerControlSystem(_inputType);
            _mainFactory = new MainFactory(_firstRoadSpan);
        }

        private void AssignConnections()
        {
            _playerController.OnChoosingPlayer += _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin += _settingsController.CoinCounter.AddCoins;
        }

        private void SignOffConnections()
        {
            _playerController.OnChoosingPlayer -= _cameraFollow.SetTarget;
            _playerController.TriggerHandler.OnTriggeredByCoin -= _settingsController.CoinCounter.AddCoins;
        }

        private void OnDestroy()
        {
            _mainFactory.Dispose();
            SignOffConnections();
        }
    }
}