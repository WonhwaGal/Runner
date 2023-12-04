namespace GameUI
{
    internal sealed class GameUIController
    {
        private readonly GameCanvas _gameCanvas;
        private readonly IPauseView _pauseView;

        public GameUIController(GameCanvas gameCanvas)
        {
            _gameCanvas = gameCanvas;
            _pauseView = gameCanvas.PauseView;
            _gameCanvas.PauseView.Init();
        }

        public IPauseView PauseView => _pauseView;

        public void PauseGame(bool isPaused)
        {
            if (isPaused)
            {
                _gameCanvas.PauseView.gameObject.SetActive(true);
                _gameCanvas.PauseView.ShowPauseMenu();
            }
            else
                _gameCanvas.PauseView.HidePauseMenu();

            _gameCanvas.GameUIView.PauseGame(isPaused);
        }
    }
}