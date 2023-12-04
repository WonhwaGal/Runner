using UnityEngine;

namespace GameUI
{
    internal class GameCanvas : MonoBehaviour
    {
        [SerializeField] private GameUIView _gameUiView;
        [SerializeField] private PauseView _pauseView;

        public GameUIView GameUIView => _gameUiView;
        public PauseView PauseView => _pauseView;
    }
}