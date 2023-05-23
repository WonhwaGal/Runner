using System.Collections.Generic;
using System;
using UnityEngine;
using PlayerSystem;

namespace ProgressSystem
{
    [CreateAssetMenu(fileName = nameof(GameProgressConfig), menuName = "MyConfigs/GameProgressConfig")]
    internal class GameProgressConfig : ScriptableObject
    {
        [Serializable]
        internal class PlayerConfig
        {
            public string Name;
            public PlayerController Player;
            public bool CanJump;
            public Sprite PlayerImage;
            public Sprite CloseImage;
            public bool IsDefault;
            public bool IsCurrent;
            public bool IsOpen;
            public int CoinPrice;
        }
        public string DefaultPlayerName;
        public int TotalCoinCount;
        public List<PlayerConfig> Players = new List<PlayerConfig>();
    }
}