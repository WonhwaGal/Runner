using GameUI;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal class ProgressController : IProgressController
    {
        private GameUIView _uiView;
        private GameUIModel _uiModel;
        private CollectableCounter _collectableCounter;
        private GameProgressConfig _gameConfig;

        public GameUIModel UIModel { get => _uiModel; }
        public CollectableCounter CollectableCounter { get => _collectableCounter; }
        public GameProgressConfig GameConfig { get => _gameConfig; }

        public ProgressController(GameUIView uiView, GameProgressConfig gameConfig)
        {
            _uiView = uiView;
            _gameConfig = gameConfig;
            _uiModel = new GameUIModel();
            _collectableCounter = new CollectableCounter();
            UIModel.OnChangeKM += _uiView.SetDistance;
            CollectableCounter.OnCollectCoins += _uiView.SetCoinNumber;
            CollectableCounter.OnCollectCrystals += _uiView.SetCrystalNumber;
            CollectableCounter.SetCoinNumber(_gameConfig.TotalCoinCount, _gameConfig.TotalCrystalCount);
        }

        public PlayerConfig RecieveCurrentPlayer()
        {
            PlayerConfig defaultPlayer = null;
            for(int i = 0; i < GameConfig.Players.Count; i++)
            {
                if (GameConfig.Players[i].IsCurrent && GameConfig.Players[i].IsOpen)
                    return GameConfig.Players[i];
                if (GameConfig.Players[i].IsDefault)
                    defaultPlayer = GameConfig.Players[i];
            }
            return defaultPlayer;
        }
        public void RegisterCurrentProgress()
        {
            _gameConfig.TotalCoinCount = _collectableCounter.SaveCurrentCoinNumber();
            _gameConfig.TotalCrystalCount = _collectableCounter.SaveCurrentCrystalNumber();
        }

        public void Dispose()
        {
            UIModel.OnChangeKM -= _uiView.SetDistance;
            CollectableCounter.OnCollectCoins -= _uiView.SetCoinNumber;
            CollectableCounter.OnCollectCrystals -= _uiView.SetCrystalNumber;
        }
    }
}