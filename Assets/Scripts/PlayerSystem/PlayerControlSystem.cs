using Factories;
using Infrastructure;
using UnityEngine;
using DG.Tweening;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal class PlayerControlSystem: IPlayerControlSystem
    {
        private IInput _inputType;
        private RoadSystem _roadSystem;
        private TriggerHandler _triggerHandler;
        private PlayerUpgradeController _upgrader;
        private PlayerController _playerController;
        private CameraFollow _cameraFollow;
        public TriggerHandler TriggerHandler { get => _triggerHandler; set => _triggerHandler = value; }

        public PlayerControlSystem(IInput inputType, RoadSystem roadSystem, CameraFollow cameraFollow)
        {
            _inputType = inputType;
            _upgrader = new PlayerUpgradeController();
            _roadSystem = roadSystem;
            _cameraFollow = cameraFollow;

            _triggerHandler = new TriggerHandler(_upgrader);
            _triggerHandler.OnGettingUpgrade += _upgrader.ActivateUpgrade;
            _triggerHandler.OnHittingAnObstacle += _cameraFollow.ShakeCamera;
        }

        public void CreatePlayer(PlayerConfig config)
        {
            _playerController = GameObject.Instantiate(config.Player);
            _playerController.Initialize(_inputType, config.CanJump, TriggerHandler);

            _roadSystem.StartRoadSpawn();
            _cameraFollow.SetTarget(_playerController.transform);
        }
        public void PausePlayer(bool pauseOn)
        {
            if (pauseOn)
                DOTween.PauseAll();
            else
                DOTween.PlayAll();
        }

        public void StopPlayer()
        {
            _playerController.StopPlayerMove();
            _roadSystem.StopRoadSpawn();
        }

        public void Dispose() => _triggerHandler.OnGettingUpgrade -= _upgrader.ActivateUpgrade;
    }
}