using DataSaving;


namespace GameUI
{
    internal class MainMenuController: IMenuController
    {
        private MainMenuCanvas _menuCanvas;
        private SelectMenuPresenter _selectPresenter;
        private ISelectLogic _selectLogic;
        private DataController _dataController;
        public ISelectLogic SelectLogic { get => _selectLogic; }

        public MainMenuController(MainMenuCanvas menuCanvas, DataController dataController)
        {
            _menuCanvas = menuCanvas;
            _dataController = dataController;
            _selectLogic = new SelectMenuLogic();
            _selectPresenter = new SelectMenuPresenter(_menuCanvas.SelectPlayerView, SelectLogic);
            _selectLogic.OnChangingGameCfg += _dataController.SaveProgressFromConfig;
            _menuCanvas.Init(_selectPresenter, SelectLogic.SelectCurrentPlayer, SelectLogic.CancelProgress);
        }


        public void Dispose()
        {
            _selectPresenter.Dispose();
            _selectLogic.OnChangingGameCfg -= _dataController.SaveProgressFromConfig;
        } 
    }
}