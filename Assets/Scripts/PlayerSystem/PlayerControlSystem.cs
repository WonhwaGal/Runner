using Infrastructure;
using System;
using UnityEngine;
using static ProgressSystem.PlayerCfgList;


namespace PlayerSystem
{
    internal class PlayerControlSystem
    {
        public Action<Transform> OnChoosingPlayer;

        private IInput _inputType;
        private TriggerHandler _triggerHandler;
        private PlayerUpgradeController _upgrader;
        private PlayerController _playerController;
        public PlayerControlSystem(IInput inputType)
        {
            _inputType = inputType;
            _upgrader = new PlayerUpgradeController();
            TriggerHandler = new TriggerHandler(_upgrader);

            _triggerHandler.OnGettingUpgrade += _upgrader.ActivateUpgrade;
        }

        public TriggerHandler TriggerHandler { get => _triggerHandler; set => _triggerHandler = value; }

        public void CreatePlayer(PlayerConfig config)
        {
            _playerController = GameObject.Instantiate(config.Player);
            _playerController.Initialize(_inputType, config.CanJump, TriggerHandler);

            OnChoosingPlayer?.Invoke(_playerController.transform);
        }

        public void StopPlayer() => _playerController.StopPlayerMove();


        public void Dispose()
        {
            _triggerHandler.OnGettingUpgrade -= _upgrader.ActivateUpgrade;
        }
    }
}