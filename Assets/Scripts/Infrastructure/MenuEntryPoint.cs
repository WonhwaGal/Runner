using DataSaving;
using GameUI;
using ProgressSystem;
using UnityEngine;

namespace Infrastructure
{
    public class MenuEntryPoint : MonoBehaviour
    {
        [SerializeField] private MainMenuCanvas _menuCanvas;
        [SerializeField] private GameProgressConfig _playerList;

        private MainMenuController _menuController;
        private DataController _dataController;

        private void Start()
        {
            _dataController = new DataController(_playerList);
            _menuController = new MainMenuController(_menuCanvas, _dataController);
            _dataController.Init(_menuController.SelectLogic);
        }

        private void OnDestroy() => _menuController.Dispose();
    }
}