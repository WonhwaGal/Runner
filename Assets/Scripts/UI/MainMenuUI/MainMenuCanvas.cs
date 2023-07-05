using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


namespace GameUI
{
    internal class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private SelectPlayerView _selectPlayerView;
        [SerializeField] private GameObject _startMenuPanel;
        [SerializeField] private GameObject _selectMenuPanel;

        [SerializeField] private TextButtonPanel _startButton;
        [SerializeField] private TextButtonPanel _selectPlayerPanelButton;
        [SerializeField] private TextButtonPanel _resetProgressButton;
        [SerializeField] private TextButtonPanel _exitButton;
        [SerializeField] private Button _backButton;

        private List<TextButtonPanel> _textPanelList;
        private UnityAction _startMethod;
        private UnityAction _resetMethod;
        public SelectPlayerView SelectPlayerView { get => _selectPlayerView; private set => _selectPlayerView = value; }
        public List<TextButtonPanel> TextPanelList { get => _textPanelList; }

        public Action<bool> OnGoSelectPlayer;

        private void Start()
        {
            _textPanelList = new List<TextButtonPanel>();
            _selectMenuPanel.gameObject.SetActive(false);
            _backButton.gameObject.SetActive(false);

            AssignTextButtons();
        }

        private void AssignTextButtons()
        {
            _startButton.OnClickButton += () => _startMenuPanel.SetActive(false);
            TextPanelList.Add(_startButton);
            _selectPlayerPanelButton.OnClickButton += GoToSelectPlayer;
            TextPanelList.Add(_selectPlayerPanelButton);
            _exitButton.OnClickButton += Exit;
            TextPanelList.Add(_exitButton);
            _backButton.onClick.AddListener(GoBackToMenu);
        }

        public void Init(SelectMenuPresenter selectPresenter, UnityAction startMethod, UnityAction resetMethod)
        {
            _selectPlayerView.Init(selectPresenter);
            _startMethod = startMethod;
            _startButton.OnClickButton += () => _startMethod();
            _resetMethod = resetMethod;
            _resetProgressButton.OnClickButton += () => _resetMethod();
            TextPanelList.Add(_resetProgressButton);
        }
        private void GoToSelectPlayer()
        {
            OnGoSelectPlayer?.Invoke(false);
            _startMenuPanel.SetActive(false);
            _selectMenuPanel.SetActive(true);
            _selectPlayerView.UpdatePlayerPanel();
            _backButton.gameObject.SetActive(true);
        }

        private void GoBackToMenu()
        {
            OnGoSelectPlayer?.Invoke(true);
            _startMenuPanel.SetActive(true);
            _selectMenuPanel.SetActive(false);
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
            _startButton.OnClickButton -= () => _startMethod();
            _startButton.OnClickButton -= () => _startMenuPanel.SetActive(false);
            _selectPlayerPanelButton.OnClickButton -= GoToSelectPlayer;
            _resetProgressButton.OnClickButton -= () => _resetMethod();
            _exitButton.OnClickButton -= Exit;
            _backButton.onClick.RemoveAllListeners();
        }
    }
}