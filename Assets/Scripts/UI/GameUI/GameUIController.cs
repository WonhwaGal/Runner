namespace GameUI
{
    internal class GameUIController: IUiController
    {
        private GameCanvas _gameCanvas;
        private PauseView _pauseView;

        public PauseView PauseView { get => _pauseView; private set => _pauseView = value; }
        public GameUIController(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
            _pauseView = gameCanvas.PauseView;
            _gameCanvas.PauseView.Init();
        }

        public void PauseGame(bool isPaused)
        {
            _gameCanvas.PauseView.gameObject.SetActive(isPaused);
            _gameCanvas.GameUIView.PauseGame(isPaused);
        }
    }
}