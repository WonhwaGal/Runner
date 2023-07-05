using Factories;
using Infrastructure;
using System;
using UnityEngine;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal class PlayerControlSystem : IPlayerControlSystem
    {
        public event Action OnPlayerControllerSet;

        private IInput _inputType;
        private IRoadSystem _roadSystem;
        private TriggerHandler _triggerHandler;
        private PlayerUpgradeController _upgrader;
        private PlayerController _playerController;
        private CameraFollow _cameraFollow;

        public TriggerHandler TriggerHandler { get => _triggerHandler; set => _triggerHandler = value; }
        public IPlayerController PlayerController { get => _playerController; }

        public PlayerControlSystem(IInput inputType, IRoadSystem roadSystem, CameraFollow cameraFollow)
        {
            _inputType = inputType;
            _upgrader = new PlayerUpgradeController();
            _roadSystem = roadSystem;
            _cameraFollow = cameraFollow;

            _triggerHandler = new TriggerHandler(_upgrader);
            _triggerHandler.OnGettingUpgrade += _upgrader.ActivateUpgrade;
            _triggerHandler.OnHittingAnObstacle += _cameraFollow.ShakeCamera;
            _roadSystem.RouteAnalyzer.OnLaneChangingBlocked += _inputType.IgnoreInput;
        }


        public void CreatePlayer(PlayerConfig config)
        {
            UpdateLimitedUse(config);
            _playerController = GameObject.Instantiate(config.Player);
            _playerController.Initialize(_inputType, config.JumpForce, TriggerHandler);

            SetConnections(config);
        }

        private void SetConnections(PlayerConfig config)
        {
            OnPlayerControllerSet?.Invoke();

            _playerController.TriggerModule.OnTriggeredByRoadSpan += _roadSystem.RouteAnalyzer.CheckForTurn;
            _playerController.Mover.OnChangingLane += _roadSystem.UpdatePlayerLane;

            _cameraFollow.SetTarget(_playerController.transform);

            _triggerHandler.Init(config);
            _triggerHandler.OnHittingAnObstacle += _playerController.PlayerAnimator.FallDown;
        }

        private void UpdateLimitedUse(PlayerConfig config)
        {
            if (config.LimitedUse)
            {
                config.TimesLeftToPlay--;
                if (config.TimesLeftToPlay == 0)
                    config.IsOpen = false;
            }
        }

        public void PausePlayer(bool pauseOn)
        {
            if (pauseOn)
                _playerController.PausePlayerMove();
            else
                _playerController.ResumePlayerMove();
        }

        public void StopPlayer() => _playerController.StopPlayerMove();

        public void Dispose()
        {
            _triggerHandler.OnGettingUpgrade -= _upgrader.ActivateUpgrade;
            _roadSystem.RouteAnalyzer.OnLaneChangingBlocked -= _inputType.IgnoreInput;
            _playerController.TriggerModule.OnTriggeredByRoadSpan -= _roadSystem.RouteAnalyzer.CheckForTurn;
            _playerController.Mover.OnChangingLane -= _roadSystem.UpdatePlayerLane;
            if (_playerController != null)
                _triggerHandler.OnHittingAnObstacle -= _playerController.PlayerAnimator.FallDown;
        }
    }
}