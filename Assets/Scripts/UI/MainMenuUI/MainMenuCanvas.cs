using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace GameUI
{
    internal class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private SelectPlayerView _selectPlayerView;
        [SerializeField] private GameObject _startMenuPanel;

        [SerializeField] private Button _startButton;
        [SerializeField] private Button _selectPlayerPanelButton;
        [SerializeField] private Button _cancelProgressButton;
        [SerializeField] private Button _exitButton;
        [SerializeField] private Button _backButton;

        public Action OnStartingGame;
        internal SelectPlayerView SelectPlayerView { get => _selectPlayerView; private set => _selectPlayerView = value; }
        public Button StartButton { get => _startButton; set => _startButton = value; }
        public Button ExitButton { get => _exitButton; set => _exitButton = value; }

        private void Start()
        {
            _selectPlayerView.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
            _startButton.onClick.AddListener(() => _startMenuPanel.SetActive(false));
            _selectPlayerPanelButton.onClick.AddListener(GoToSelectPlayer);
            _backButton.onClick.AddListener(GoBackToMenu);
            _exitButton.onClick.AddListener(Exit);
        }

        public void Init(SelectMenuPresenter selectPresenter, UnityAction startMethod, UnityAction cancelMethod)
        {
            _selectPlayerView.Init(selectPresenter);
            StartButton.onClick.AddListener(startMethod);
            _cancelProgressButton.onClick.AddListener(cancelMethod);
        }
        private void GoToSelectPlayer()
        {
            _startMenuPanel.SetActive(false);
            _selectPlayerView.gameObject.SetActive(true);
            _selectPlayerView.FillInPlayerPanel();
            _backButton.gameObject.SetActive(true);
        }

        private void GoBackToMenu()
        {
            _startMenuPanel.SetActive(true);
            _selectPlayerView.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);
        }
        private void Exit()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
                UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
            Application.Quit();
#endif
        }
        private void OnDestroy()
        {
            StartButton.onClick.RemoveAllListeners();
            _selectPlayerPanelButton.onClick.RemoveAllListeners();
            _backButton.onClick.RemoveAllListeners();
            _cancelProgressButton.onClick.RemoveAllListeners(); 
            _exitButton.onClick.RemoveAllListeners();
        }
    }
}