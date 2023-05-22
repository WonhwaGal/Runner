using GameUI;
using static ProgressSystem.PlayerCfgList;

namespace ProgressSystem
{
    internal class ProgressController
    {
        private GameUIView _uiView;
        private GameUIModel _uiModel;
        private CoinCounter _coinCounter;
        public ProgressController(GameUIView uiView)
        {
            _uiView = uiView;
            _uiModel = new GameUIModel();
            _coinCounter = new CoinCounter();
            UIModel.OnChangeKM += _uiView.SetDistance;
            CoinCounter.OnCollectCoins += _uiView.SetCoinNumber;
            
        }

        public GameUIModel UIModel { get => _uiModel; }
        public CoinCounter CoinCounter { get => _coinCounter; }

        public int GetCurrentProgress()
        {
            _uiModel.StopDistanceCount();
            return _coinCounter.SaveCurrentCoinNumber();
        }
        public void SpendCoinsOnPlayer(PlayerConfig config) => _coinCounter.AddCoins(config.CoinPrice * -1);

        public void Dispose()
        {
            UIModel.OnChangeKM -= _uiView.SetDistance;
            CoinCounter.OnCollectCoins -= _uiView.SetCoinNumber;
        }
    }
}