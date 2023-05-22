

namespace GameUI
{
    internal class UIController: IUiController
    {
        private MainCanvas _mainCanvas;
        private SelectMenuPresenter _selectPresenter;
        private SelectMenuLogic _selectLogic;
        public SelectMenuLogic SelectLogic { get => _selectLogic; }
        public UIController(MainCanvas mainCanvas)
        {
            _mainCanvas = mainCanvas;
            _selectLogic = new SelectMenuLogic();
            _selectPresenter = new SelectMenuPresenter(_mainCanvas.SelectPlayerView, SelectLogic);
            _mainCanvas.Init(_selectPresenter, SelectLogic.SelectCurrentPlayer, SelectLogic.CancelProgress);
        }
        public void PauseGame(bool isPaused) => _mainCanvas.PauseON(isPaused);

        public void Dispose() => _selectPresenter.Dispose();
    }
}