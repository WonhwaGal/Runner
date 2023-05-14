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
        [SerializeField] private PlayerCfgList _playerTypes;

        private IInput _inputType;
        private TimerController _timerController;

        private PlayerController _playerController;
        private SettingsController _settingsController;
        private MainFactory _mainFactory;
        
        private void Start()
        {
            _inputType = new KeyboardInput();
            _timerController = new TimerController();

            _settingsController = new SettingsController(_playerTypes);
            _playerController = new PlayerController(_inputType);
            _mainFactory = new MainFactory(_timerController.Timers, _firstRoadSpan);

            // assign settings Action of choosing the player to the PlayerController.CreatePlayer();
            _playerController.OnChoosingPlayer += _cameraFollow.SetTarget;
            _settingsController.OnChangingPlayer += _playerController.CreatePlayer;

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))   // TO DELETE
                _settingsController.CurrentPlayer = _playerTypes.Players[1];
            if (Input.GetKeyDown(KeyCode.R))
                _settingsController.CurrentPlayer = _playerTypes.Players[0];

                _timerController.Record(Time.deltaTime);
        }
        private void OnDestroy()
        {
            _timerController.Timers.Clear();
            _mainFactory.Dispose();
            _playerController.OnChoosingPlayer -= _cameraFollow.SetTarget;
            _settingsController.OnChangingPlayer -= _playerController.CreatePlayer;
        }
    }
}