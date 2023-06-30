using GameUI;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal class ProgressController : IProgressController
    {
        private IGameUIView _uiView;
        private IGameUIModel _gameUIModel;
        private ICollectableCounter _collectableCounter;
        private GameProgressConfig _gameConfig;

        public IGameUIModel GameUIModel { get => _gameUIModel; }
        public ICollectableCounter CollectableCounter { get => _collectableCounter; }
        public GameProgressConfig GameConfig { get => _gameConfig; }

        public ProgressController(IGameUIView uiView, GameProgressConfig gameConfig)
        {
            _uiView = uiView;
            _gameConfig = gameConfig;
            _gameUIModel = new GameUIModel();
            _collectableCounter = new CollectableCounter();
            _gameUIModel.OnChangeKM += _uiView.SetDistance;
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
            _gameConfig.TotalCoinCount = _collectableCounter.SetCurrentCoinNumber();
            _gameConfig.TotalCrystalCount = _collectableCounter.SetCurrentCrystalNumber();
        }

        public void Dispose()
        {
            GameUIModel.OnChangeKM -= _uiView.SetDistance;
            CollectableCounter.OnCollectCoins -= _uiView.SetCoinNumber;
            CollectableCounter.OnCollectCrystals -= _uiView.SetCrystalNumber;
        }
    }
}