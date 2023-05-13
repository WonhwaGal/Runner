using Infrastructure;
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
    }
}