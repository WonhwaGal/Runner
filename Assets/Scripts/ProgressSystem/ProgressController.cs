using GameUI;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal class ProgressController : IProgressController
    {
        private GameUIView _uiView;
        private GameUIModel _uiModel;
        private CoinCounter _coinCounter;
        private GameProgressConfig _gameConfig;

        public GameUIModel UIModel { get => _uiModel; }
        public CoinCounter CoinCounter { get => _coinCounter; }
        public GameProgressConfig GameConfig { get => _gameConfig; }

        public ProgressController(GameUIView uiView, GameProgressConfig gameConfig)
        {
            _uiView = uiView;
            _gameConfig = gameConfig;
            _uiModel = new GameUIModel();
            _coinCounter = new CoinCounter();
            UIModel.OnChangeKM += _uiView.SetDistance;
            CoinCounter.OnCollectCoins += _uiView.SetCoinNumber;
            CoinCounter.SetCoinNumber(_gameConfig.TotalCoinCount);
        }

        public PlayerConfig RecieveCurrentPlayer()
        {
            for(int i = 0; i < GameConfig.Players.Count; i++)
            {
                if (GameConfig.Players[i].IsCurrent)
                    return GameConfig.Players[i];
            }
            return GameConfig.Players[0];
        }
        public void RegisterCurrentProgress() 
            => _gameConfig.TotalCoinCount = _coinCounter.SaveCurrentCoinNumber();

        public void Dispose()
        {
            UIModel.OnChangeKM -= _uiView.SetDistance;
            CoinCounter.OnCollectCoins -= _uiView.SetCoinNumber;
        }
    }
}