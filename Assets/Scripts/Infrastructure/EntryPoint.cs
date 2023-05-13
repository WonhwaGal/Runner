using Factories;
using PlayerSystem;
using Tools;
using UnityEngine;

namespace Infrastructure
{
    public class EntryPoint : MonoBehaviour
    {
        [SerializeField] private Transform _firstRoadSpan;
        //[SerializeField] private CameraFollow _cameraFollow;

        private IInput _inputType;

        private PlayerController _playerController;
        private SettingsController _settingsController;
        private TimerController _timerController;
        private MainFactory _mainFactory;
        
        private void Start()
        {
            _inputType = new KeyboardInput();
            _playerController = new PlayerController(_inputType);
            _settingsController = new();
            _timerController = new TimerController();
            _mainFactory = new MainFactory(_timerController.Timers, _firstRoadSpan);

            // assign settings Action of choosing the player to the PlayerController.CreatePlayer();
            //_playerController.OnChoosingPlayer += _cameraFollow.SetTarget;

        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))   // TO DELETE
                _playerController.CreatePlayer();

            _timerController.Record(Time.deltaTime);
        }
        private void OnDestroy()
        {
            _timerController.Timers.Clear();
            _mainFactory.Dispose();
            //_playerController.OnChoosingPlayer -= _cameraFollow.SetTarget;
        }
    }
}