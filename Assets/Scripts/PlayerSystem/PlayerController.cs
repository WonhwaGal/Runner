using Infrastructure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerSystem
{
    internal class PlayerController
    {
        private string playerPrefabPath = "Player";

        private IInput _inputType;
        public PlayerController(IInput inputType)
        {
            _inputType = inputType;
        }

        public void CreatePlayer()
        {
            PlayerMover player = Object.Instantiate(Resources.Load<PlayerMover>(playerPrefabPath));
            player.AssignInput(_inputType);

            Camera.main.transform.parent = player.transform;
        }
    }
}