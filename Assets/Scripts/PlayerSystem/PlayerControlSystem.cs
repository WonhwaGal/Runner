using System;
using UnityEngine;
using Factories;
using Infrastructure;
using static ProgressSystem.GameProgressConfig;

namespace PlayerSystem
{
    internal class PlayerControlSystem : IPlayerControlSystem
    {
        private readonly IInput _inputType;
        private readonly IRoadSystem _roadSystem;
        private readonly TriggerHandler _triggerHandler;
        private readonly PlayerUpgradeController _upgrader;
        private PlayerController _playerController;
        private readonly CameraFollow _cameraFollow;

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

        public TriggerHandler TriggerHandler => _triggerHandler;
        public IPlayerController PlayerController => _playerController;

        public Action OnPlayerControllerSet { get; set; }

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
            _playerController.Mover.OnSpeedingUp += _roadSystem.SpeedUp;

            _cameraFollow.SetTarget(_playerController.transform);

            _triggerHandler.Init(config);
            _triggerHandler.OnHittingAnObstacle += _playerController.PlayerAnimator.FallDown;
        }

        private void UpdateLimitedUse(PlayerConfig config)
        {
            if (config.LimitedUse)
            {
                config.TimesLeftToPlay--;
                if (config.TimesLeftToPlay <= 0)
                {
                    config.IsOpen = false;
                    config.TimesLeftToPlay = 0;
                }
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
            _triggerHandler.OnHittingAnObstacle -= _cameraFollow.ShakeCamera;
            _roadSystem.RouteAnalyzer.OnLaneChangingBlocked -= _inputType.IgnoreInput;
            _playerController.TriggerModule.OnTriggeredByRoadSpan -= _roadSystem.RouteAnalyzer.CheckForTurn;
            _playerController.Mover.OnChangingLane -= _roadSystem.UpdatePlayerLane;
            _playerController.Mover.OnSpeedingUp -= _roadSystem.SpeedUp;
            if (_playerController != null)
                _triggerHandler.OnHittingAnObstacle -= _playerController.PlayerAnimator.FallDown;
        }
    }
}