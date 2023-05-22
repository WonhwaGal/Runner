using System;
using static ProgressSystem.PlayerCfgList;

namespace GameUI
{
    internal class SelectMenuPresenter
    {
        SelectPlayerView _selectView;
        SelectMenuLogic _selectLogic;
        public SelectMenuPresenter(SelectPlayerView selectView, SelectMenuLogic selectLogic)
        {
            _selectView = selectView;
            _selectLogic = selectLogic;
            _selectLogic.AssignPlayerConfig(selectView.PlayerTypes);
            
            _selectLogic.OnSettingCoinNumber += UpdateCoinNumber;
        }

        public void AssignSelectedPlayer(PlayerConfig config) => _selectLogic.SelectChosenPlayer(config);
        public void BuyPlayer(PlayerConfig config) => _selectLogic.BuyPlayer(config);
        public void OpenNewPlayer(PlayerConfig config) => _selectLogic.AddNewPlayer(config);
        private void UpdateCoinNumber(int number) => _selectView.SetCoinNumber(number);

        public void Dispose()
        {
            _selectLogic.OnSettingCoinNumber -= UpdateCoinNumber;
        }
    }
}