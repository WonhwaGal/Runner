using UnityEngine;

namespace GameUI
{
    internal class MainCanvas : MonoBehaviour
    {
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private GameUIView _gameUIView;

        public GameUIView GameUIView { get => _gameUIView; private set => _gameUIView = value; }
        internal MainMenuView MainMenuView { get => _mainMenuView; private set => _mainMenuView = value; }
    }
}