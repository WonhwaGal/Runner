using Infrastructure;
using SettingsSystem;
using System;
using UnityEngine;

namespace PlayerSystem
{
    internal class PlayerController
    {
        public Action<Transform> OnChoosingPlayer;

        private string playerPrefabPath = "Jumper";

        private IInput _inputType;
        private PlayerJumper _playerJumper;
        private PlayerConfig _config;

        public PlayerController(IInput inputType)
        {
            _inputType = inputType;
        }

        public void CreatePlayer()
        {
            PlayerMover player = GameObject.Instantiate(Resources.Load<PlayerMover>(playerPrefabPath));
            player.AssignInput(_inputType);

            _playerJumper = player.GetComponent<PlayerJumper>();
            _playerJumper.AssignInput(_inputType);

            OnChoosingPlayer?.Invoke(player.transform);
        }

        public void CreatePlayer(PlayerConfig config)
        {
            Debug.Log("came to player");
            PlayerMover player = GameObject.Instantiate(config.PlayerMover);
            player.AssignInput(_inputType);

            if (config.CanJump)
            {
                _playerJumper = player.GetComponent<PlayerJumper>();
                _playerJumper.AssignInput(_inputType);
            }

            OnChoosingPlayer?.Invoke(player.transform);
        }
    }
}