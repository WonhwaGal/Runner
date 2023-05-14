using System.Collections.Generic;
using UnityEngine;

namespace SettingsSystem
{
    [CreateAssetMenu(fileName = nameof(PlayerCfgList), menuName = "MyConfigs/PlayerCfgList")]
    internal class PlayerCfgList : ScriptableObject
    {
        public List<PlayerConfig> Players = new List<PlayerConfig>();
    }
}