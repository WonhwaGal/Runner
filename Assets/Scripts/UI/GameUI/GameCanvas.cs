using UnityEngine;

namespace GameUI
{
    public class GameCanvas : MonoBehaviour
    {
        [SerializeField] private GameUIView _gameUiView;
        [SerializeField] private PauseView _pauseView;

        public GameUIView GameUIView { get => _gameUiView; private set => _gameUiView = value; }
        public PauseView PauseView { get => _pauseView; private set => _pauseView = value; }
    }
}