using DataSaving;
using GameUI;
using ProgressSystem;
using UnityEngine;

namespace Infrastructure
{
    internal class MenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private MainMenuCanvas _menuCanvas;

        private MainMenuController _menuController;
        private DataController _dataController;

        private void Start()
        {
            _dataController = new DataController();
            _menuController = new MainMenuController(_menuCanvas, _dataController);
            _dataController.Init(_menuController.SelectLogic);
        }

        private void OnDestroy() => _menuController.Dispose();
    }
}