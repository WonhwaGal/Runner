using System;
using DataSaving;
using ProgressSystem;
using static ProgressSystem.GameProgressConfig;

namespace GameUI
{
    internal interface ISelectLogic
    {
        event Action OnPlayerSelected;
        event Action<int> OnSettingCoinNumber;
        event Action<int> OnSettingCrystalNumber;
        event Action<GameProgressConfig> OnChangingGameCfg;

        void AssignPlayerConfig(GameProgressConfig gameConfig);
        void UpdatePlayersConfig(ProgressSavedData savedData);
        void ChangeCurrentPlayerTo(PlayerConfig config);
        void BuyPlayer(PlayerConfig config);
        void CancelProgress();
    }
}