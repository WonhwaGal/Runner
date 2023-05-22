using Factories;
using Infrastructure;
using System;
using UnityEngine;
using DG.Tweening;
using static ProgressSystem.PlayerCfgList;


namespace PlayerSystem
{
    internal class PlayerControlSystem
    {
        public Action<Transform> OnChoosingPlayer;
        public Action OnGameStart;

        private IInput _inputType;
        private RoadSystem _roadSystem;
        private TriggerHandler _triggerHandler;
        private PlayerUpgradeController _upgrader;
        private PlayerController _playerController;
        public PlayerControlSystem(IInput inputType, RoadSystem roadSystem)
        {
            _inputType = inputType;
            _upgrader = new PlayerUpgradeController();
            _roadSystem = roadSystem;
            TriggerHandler = new TriggerHandler(_upgrader);

            _triggerHandler.OnGettingUpgrade += _upgrader.ActivateUpgrade;
            OnGameStart += roadSystem.StartRoadSpawn;
        }

        public TriggerHandler TriggerHandler { get => _triggerHandler; set => _triggerHandler = value; }

        public void CreatePlayer(PlayerConfig config)
        {
            _playerController = GameObject.Instantiate(config.Player);
            _playerController.Initialize(_inputType, config.CanJump, TriggerHandler);

            OnChoosingPlayer?.Invoke(_playerController.transform);
            OnGameStart?.Invoke();
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

        public void Dispose()
        {
            _triggerHandler.OnGettingUpgrade -= _upgrader.ActivateUpgrade;
            OnGameStart -= _roadSystem.StartRoadSpawn;
        }
    }
}