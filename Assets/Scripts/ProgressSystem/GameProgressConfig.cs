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
            public string Description;
            public PlayerController Player;
            public float JumpForce;
            public Sprite PlayerImage;
            public bool IsDefault;
            public bool IsCurrent;
            public bool IsOpen;
            public bool LimitedUse;
            public int TimesToPlay;
            public int TimesLeftToPlay;
            public int CoinPrice;
            public float UpgradeMultiplier = 1;
            public int CoinMultiplier = 1;
            public int CrystalMultiplier = 1;
        }
        public string DefaultPlayerName;
        public int TotalCoinCount;
        public int TotalCrystalCount;
        public List<PlayerConfig> Players = new List<PlayerConfig>();
    }
}