using PlayerSystem;
using System;
using UnityEngine;

namespace SettingsSystem
{
    //[CreateAssetMenu(fileName = nameof(PlayerConfig), menuName = "MyConfigs/PlayerConfig")]
    [Serializable]
    internal class PlayerConfig
    {
        public PlayerMover PlayerMover;
        public bool CanJump;
        public string Name;
        public Sprite Image;
    }
}