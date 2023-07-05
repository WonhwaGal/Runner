using DataSaving;
using GameUI;
using MainMenu;
using UnityEngine;

namespace Infrastructure
{
    internal class MenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private MainMenuCanvas _menuCanvas;
        [SerializeField] private Transform _menuCharacterSpot;

        private IMenuController _menuController;
        private DataController _dataController;

        private void Start()
        {
            _dataController = new DataController();
            _menuController = new MainMenuController(_menuCanvas, _dataController, _menuCharacterSpot);
            _dataController.Init(_menuController.SelectLogic);
        }

        private void OnDestroy() => _menuController.Dispose();
    }
}