using UnityEngine;
using DataSaving;
using GameUI;

namespace MainMenu
{
    internal class MainMenuController: IMenuController
    {
        private readonly MainMenuCanvas _menuCanvas;
        private readonly SelectMenuPresenter _selectPresenter;
        private readonly ISelectLogic _selectLogic;
        private readonly DataController _dataController;
        private readonly MenuCharacterLoader _menuCharacterLoader;

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

        public ISelectLogic SelectLogic => _selectLogic;

        public void Dispose()
        {
            _selectPresenter.Dispose();
            _selectLogic.OnChangingGameCfg -= _dataController.SaveProgressFromConfig;
            _menuCanvas.OnGoSelectPlayer -= _menuCharacterLoader.ShowMenuPlayer;
        }
    }
}