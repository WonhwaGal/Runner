using GameUI;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal sealed class ProgressController : IProgressController
    {
        private readonly IGameUIView _uiView;
        private readonly IGameUIModel _gameUIModel;
        private readonly CollectableCounter _collectableCounter;
        private readonly GameProgressConfig _gameConfig;

        public ProgressController(IGameUIView uiView, GameProgressConfig gameConfig)
        {
            _uiView = uiView;
            _gameConfig = gameConfig;
            _gameUIModel = new GameUIModel();
            _collectableCounter = new ();
            _gameUIModel.OnChangeKM += _uiView.SetDistance;
            CollectableCounter.OnCollectCoins += _uiView.SetCoinNumber;
            CollectableCounter.OnCollectCrystals += _uiView.SetCrystalNumber;
            CollectableCounter.SetCoinNumber(_gameConfig.TotalCoinCount, _gameConfig.TotalCrystalCount);
        }

        public IGameUIModel GameUIModel => _gameUIModel;
        public CollectableCounter CollectableCounter => _collectableCounter;
        public GameProgressConfig GameConfig => _gameConfig;

        public PlayerConfig RecieveCurrentPlayer()
        {
            PlayerConfig nextPlayer = null;

            if (_gameConfig.CurrentPlayer != null && _gameConfig.CurrentPlayer.IsOpen)
            {
                nextPlayer = _gameConfig.CurrentPlayer;
            }
            else
            {
                for (int i = 0; i < GameConfig.Players.Count; i++)
                {
                    if (GameConfig.Players[i].IsDefault)
                        nextPlayer = GameConfig.Players[i];
                }
            }

            return nextPlayer;
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