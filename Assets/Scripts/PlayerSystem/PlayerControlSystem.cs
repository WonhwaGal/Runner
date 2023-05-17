using Infrastructure;
using SettingsSystem;
using System;
using UnityEngine;
using static SettingsSystem.PlayerCfgList;

namespace PlayerSystem
{
    internal class PlayerControlSystem
    {
        public Action<Transform> OnChoosingPlayer;

        private IInput _inputType;
        private TriggerHandler _triggerHandler;

        public PlayerControlSystem(IInput inputType)
        {
            _inputType = inputType;
            TriggerHandler = new TriggerHandler();
        }

        internal TriggerHandler TriggerHandler { get => _triggerHandler; set => _triggerHandler = value; }

        public void CreatePlayer(PlayerConfig config)
        {
            PlayerController player = GameObject.Instantiate(config.Player);
            player.Initialize(_inputType, config.CanJump, TriggerHandler);

            OnChoosingPlayer?.Invoke(player.transform);
        }
    }
}