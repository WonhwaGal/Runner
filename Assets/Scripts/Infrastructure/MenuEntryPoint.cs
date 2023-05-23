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

        private void Start()
        {
            _menuController = new MainMenuController(_menuCanvas);
            new DataController(_playerList, _menuController.SelectLogic);
        }

        private void OnDestroy() => _menuController.Dispose();
    }
}