using UnityEngine.SceneManagement;
using static ProgressSystem.GameProgressConfig;


namespace GameUI
{
    internal class SelectMenuPresenter
    {
        private readonly SelectPlayerView _selectView;
        private readonly ISelectLogic _selectLogic;

        public SelectMenuPresenter(SelectPlayerView selectView, ISelectLogic selectLogic)
        {
            _selectView = selectView;
            _selectLogic = selectLogic;
            _selectLogic.AssignPlayerConfig(selectView.PlayersConfig);
            
            _selectLogic.OnSettingCoinNumber += UpdateCoinNumber;
            _selectLogic.OnSettingCrystalNumber += UpdateCrystalNumber;
            _selectLogic.OnPlayerSelected += LoadGameScene;
        }

        public void MakePlayerCurrent(PlayerConfig config) => _selectLogic.ChangeCurrentPlayerTo(config);

        public void BuyPlayer(PlayerConfig config) => _selectLogic.BuyPlayer(config);

        private void UpdateCoinNumber(int number) => _selectView.SetCoinNumber(number);

        private void UpdateCrystalNumber(int number) => _selectView.SetCrystalNumber(number);

        public void LoadGameScene() => SceneManager.LoadScene("GameScene");

        public void Dispose()
        {
            _selectLogic.OnSettingCoinNumber -= UpdateCoinNumber;
            _selectLogic.OnSettingCrystalNumber -= UpdateCrystalNumber;
            _selectLogic.OnPlayerSelected -= LoadGameScene;
        }
    }
}