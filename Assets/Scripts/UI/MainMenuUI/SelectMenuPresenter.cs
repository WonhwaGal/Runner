using UnityEngine.SceneManagement;
using static ProgressSystem.GameProgressConfig;


namespace GameUI
{
    internal class SelectMenuPresenter
    {
        SelectPlayerView _selectView;
        ISelectLogic _selectLogic;

        public SelectMenuPresenter(SelectPlayerView selectView, ISelectLogic selectLogic)
        {
            _selectView = selectView;
            _selectLogic = selectLogic;
            _selectLogic.AssignPlayerConfig(selectView.PlayerTypes);
            
            _selectLogic.OnSettingCoinNumber += UpdateCoinNumber;
            _selectLogic.OnSettingCrystalNumber += UpdateCrystalNumber;
            _selectLogic.OnPlayerSelected += LoadSceneWithSelectedPlayer;
        }


        public void MakePlayerCurrent(PlayerConfig config) => _selectLogic.ChangeCurrentPlayerTo(config);

        public void BuyPlayer(PlayerConfig config) => _selectLogic.BuyPlayer(config);

        private void UpdateCoinNumber(int number) => _selectView.SetCoinNumber(number);

        private void UpdateCrystalNumber(int number) => _selectView.SetCrystalNumber(number);

        private void LoadSceneWithSelectedPlayer() => SceneManager.LoadScene("GameScene");

        public void Dispose()
        {
            _selectLogic.OnSettingCoinNumber -= UpdateCoinNumber;
            _selectLogic.OnSettingCrystalNumber -= UpdateCrystalNumber;
            _selectLogic.OnPlayerSelected -= LoadSceneWithSelectedPlayer;
        }
    }
}