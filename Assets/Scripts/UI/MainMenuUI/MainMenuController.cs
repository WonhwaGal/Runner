using DataSaving;
using GameUI;
using UnityEngine;

namespace MainMenu
{
    internal class MainMenuController: IMenuController
    {
        private MainMenuCanvas _menuCanvas;
        private SelectMenuPresenter _selectPresenter;
        private ISelectLogic _selectLogic;
        private DataController _dataController;
        private MenuCharacterLoader _menuCharacterLoader;
        public ISelectLogic SelectLogic { get => _selectLogic; }

        public MainMenuController(MainMenuCanvas menuCanvas, DataController dataController, Transform menuCharacterSpot)
        {
            _menuCanvas = menuCanvas;
            _dataController = dataController;

            _menuCharacterLoader 
                = new MenuCharacterLoader(_menuCanvas.SelectPlayerView.PlayersConfig, menuCharacterSpot);

            _selectLogic = new SelectMenuLogic();
            _selectPresenter = new SelectMenuPresenter(_menuCanvas.SelectPlayerView, SelectLogic);
            _selectLogic.OnChangingGameCfg += _dataController.SaveProgressFromConfig;
            _menuCanvas.OnGoSelectPlayer += _menuCharacterLoader.ShowMenuPlayer;

            _menuCanvas.Init(_selectPresenter, _selectPresenter.LoadGameScene, SelectLogic.CancelProgress);
            _menuCharacterLoader.MenuCharacter.SubscribeToButtons(_menuCanvas.TextPanelList);
        }

        public void Dispose()
        {
            _selectPresenter.Dispose();
            _selectLogic.OnChangingGameCfg -= _dataController.SaveProgressFromConfig;
        } 
    }
}