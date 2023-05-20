using GameUI;

namespace ProgressSystem
{
    internal class ProgressController
    {
        private GameUIView _uiView;
        private UIModel _uiModel;
        private CoinCounter _coinCounter;
        public ProgressController(GameUIView uiView)
        {
            _uiView = uiView;
            _uiModel = new UIModel();
            _coinCounter = new CoinCounter();
            UIModel.OnChangeKM += _uiView.SetDistance;
            CoinCounter.OnCollectCoins += _uiView.SetCoinNumber;
            
        }

        public UIModel UIModel { get => _uiModel; }
        public CoinCounter CoinCounter { get => _coinCounter; }

        public void AddCurrentProgress()
        {
            _uiModel.StopDistanceCount();
            var coins = _coinCounter.SaveCurrentCoinNumber();
        }

        public void Dispose()
        {
            UIModel.OnChangeKM -= _uiView.SetDistance;
            CoinCounter.OnCollectCoins -= _uiView.SetCoinNumber;
        }
    }
}