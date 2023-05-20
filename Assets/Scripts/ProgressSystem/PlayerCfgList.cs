using System.Collections.Generic;
using System;
using UnityEngine;
using PlayerSystem;

namespace ProgressSystem
{
    [CreateAssetMenu(fileName = nameof(PlayerCfgList), menuName = "MyConfigs/PlayerCfgList")]
    internal class PlayerCfgList : ScriptableObject
    {
        [Serializable]
        internal class PlayerConfig
        {
            public string Name;
            public PlayerController Player;
            public bool CanJump;
            public Sprite Image;
        }

        public List<PlayerConfig> Players = new List<PlayerConfig>();
    }
}