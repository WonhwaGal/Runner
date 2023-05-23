using UnityEngine.SceneManagement;
using static ProgressSystem.GameProgressConfig;

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
            _selectLogic.OnPlayerSelected += LoadSceneWithSelectedPlayer;
        }

        public void AssignSelectedPlayer(PlayerConfig config) => _selectLogic.SelectChosenPlayer(config);
        public void BuyPlayer(PlayerConfig config) => _selectLogic.BuyPlayer(config);
        private void UpdateCoinNumber(int number) => _selectView.SetCoinNumber(number);

        private void LoadSceneWithSelectedPlayer() => SceneManager.LoadScene("GameScene");
        public void Dispose()
        {
            _selectLogic.OnSettingCoinNumber -= UpdateCoinNumber;
            _selectLogic.OnPlayerSelected -= LoadSceneWithSelectedPlayer;
        }
    }
}