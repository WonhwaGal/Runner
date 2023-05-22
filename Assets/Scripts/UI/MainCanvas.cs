using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace GameUI
{
    internal class MainCanvas : MonoBehaviour
    {
        [SerializeField] private SelectPlayerView _selectPlayerView;
        [SerializeField] private GameUIView _gameUIView;
        [SerializeField] private GameObject _startMenuPanel;
        [SerializeField] private GameObject _pausePanel;
        [SerializeField] private Button _startButton;
        [SerializeField] private Button _selectPlayerPanelButton;
        [SerializeField] private Button _cancelProgressButton;
        [SerializeField] private Button _ExitButton;

        public Action OnStartingGame;
        public GameUIView GameUIView { get => _gameUIView; private set => _gameUIView = value; }
        internal SelectPlayerView SelectPlayerView { get => _selectPlayerView; private set => _selectPlayerView = value; }
        public Button StartButton { get => _startButton; set => _startButton = value; }
        public Button ExitButton { get => _ExitButton; set => _ExitButton = value; }

        private void Start()
        {
            _selectPlayerView.gameObject.SetActive(false); 
            _gameUIView.gameObject.SetActive(false);
            _pausePanel.SetActive(false);

            StartButton.onClick.AddListener(() =>
            {
                _startMenuPanel.SetActive(false);
                _gameUIView.gameObject.SetActive(true);
            });
            _selectPlayerPanelButton.onClick.AddListener(GoToSelectPlayer);
        }

        public void Init(SelectMenuPresenter selectPresenter, UnityAction startMethod, UnityAction cancelMethod)
        {
            _selectPlayerView.Init(selectPresenter);
            StartButton.onClick.AddListener(startMethod);
            _cancelProgressButton.onClick.AddListener(cancelMethod);
        }

        public void PauseON(bool isPaused) => _pausePanel.SetActive(isPaused);
        private void GoToSelectPlayer()
        {
            _startMenuPanel.SetActive(false);
            _selectPlayerView.gameObject.SetActive(true);
            _selectPlayerView.FillInPlayerPanel();
        }
    }
}