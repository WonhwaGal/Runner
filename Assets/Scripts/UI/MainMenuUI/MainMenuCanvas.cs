using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;


namespace GameUI
{
    internal class MainMenuCanvas : MonoBehaviour
    {
        [SerializeField] private SelectPlayerView _selectPlayerView;

        [Header("Panels")]
        [SerializeField] private GameObject _startMenuPanel;
        [SerializeField] private GameObject _selectMenuPanel;
        [SerializeField] private GameObject _leftStartPanel;
        [SerializeField] private GameObject _rightResetPanel;

        [Header("Buttons")]
        [SerializeField] private TextButtonPanel _startButton;
        [SerializeField] private TextButtonPanel _selectPlayerPanelButton;
        [SerializeField] private TextButtonPanel _resetProgressButton;
        [SerializeField] private TextButtonPanel _exitButton;

        [SerializeField] private Button _backButton;

        private List<TextButtonPanel> _textPanelList;
        private UnityAction _startMethod;
        private UnityAction _resetMethod;

        private const float _menuMoveSpeed = 1.5f;
        private Vector3 _hideStartVector = new (0, 0, 113);
        private Vector3 _selectStartPos = new (0, 1, 25);
        private Vector3 _selectShiftPos = new (0, 33, 25);

        public SelectPlayerView SelectPlayerView { get => _selectPlayerView; private set => _selectPlayerView = value; }
        public List<TextButtonPanel> TextPanelList => _textPanelList;

        public Action<bool> OnGoSelectPlayer;

        private void Start()
        {
            _textPanelList = new List<TextButtonPanel>();
            _selectMenuPanel.SetActive(false);
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
            TransferFromMainToSelect();
            OnGoSelectPlayer?.Invoke(false);
            _selectPlayerView.UpdatePlayerPanel();
        }

        private void GoBackToMenu()
        {
            OnGoSelectPlayer?.Invoke(true);
            TransferFromSelectToMain();
        }

        private void TransferFromMainToSelect()
        {
            _leftStartPanel.transform.DORotate(_hideStartVector, _menuMoveSpeed)
                                    .OnComplete(() => _startMenuPanel.SetActive(false));
            _rightResetPanel.transform.DORotate(_hideStartVector * -1, _menuMoveSpeed);

            _selectMenuPanel.transform.position = _selectShiftPos;
            _selectMenuPanel.SetActive(true);
            _backButton.gameObject.SetActive(true);
            _selectMenuPanel.transform.DOMoveY(_selectStartPos.y, _menuMoveSpeed);
        }

        private void TransferFromSelectToMain()
        {
            _startMenuPanel.SetActive(true);
            _leftStartPanel.transform.DORotate(new Vector3(0, 0, 0), _menuMoveSpeed);
            _rightResetPanel.transform.DORotate(new Vector3(0, 0, 0), _menuMoveSpeed);

            _selectMenuPanel.transform.DOMoveY(_selectShiftPos.y, _menuMoveSpeed)
                                    .OnComplete(() => _selectMenuPanel.SetActive(false));
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
            _startButton.Dispose();
            _selectPlayerPanelButton.Dispose();
            _resetProgressButton.Dispose();
            _exitButton.Dispose();
            _backButton.onClick.RemoveAllListeners();
        }
    }
}