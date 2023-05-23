using ProgressSystem;

namespace GameUI
{
    internal class MainMenuController: IMenuController
    {
        private MainMenuCanvas _menuCanvas;
        private SelectMenuPresenter _selectPresenter;
        private SelectMenuLogic _selectLogic;
        public SelectMenuLogic SelectLogic { get => _selectLogic; }

        public MainMenuController(MainMenuCanvas menuCanvas)
        {
            _menuCanvas = menuCanvas;
            _selectLogic = new SelectMenuLogic();
            _selectPresenter = new SelectMenuPresenter(_menuCanvas.SelectPlayerView, SelectLogic);
            _menuCanvas.Init(_selectPresenter, SelectLogic.SelectCurrentPlayer, SelectLogic.CancelProgress);
        }

        public void Dispose() => _selectPresenter.Dispose();
    }
}