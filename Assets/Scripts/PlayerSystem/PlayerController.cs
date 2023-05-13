using Infrastructure;
using System;
using UnityEngine;

namespace PlayerSystem
{
    internal class PlayerController
    {
        public Action<Transform> OnChoosingPlayer;

        private string playerPrefabPath = "Player";

        private IInput _inputType;
        private PlayerMover _playerMover; 
        public PlayerController(IInput inputType)
        {
            _inputType = inputType;
        }

        public void CreatePlayer()
        {
            PlayerMover player = GameObject.Instantiate(Resources.Load<PlayerMover>(playerPrefabPath));
            player.AssignInput(_inputType);

            _playerMover = player;
            OnChoosingPlayer?.Invoke(player.transform);
            //Camera.main.transform.parent = player.transform;
        }
    }
}