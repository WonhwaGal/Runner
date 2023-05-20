using ProgressSystem;
using System;
using UnityEngine;
using static ProgressSystem.PlayerCfgList;

namespace GameUI
{
    internal class MainMenuView : MonoBehaviour
    {
        public Action<PlayerConfig> OnSelectPlayer;

        [SerializeField] private GameObject _playerSelectMenu;
        [SerializeField] private PlayerCfgList _playerTypes;
        [SerializeField] private PlayerSlotView _playerSlotView;
        [SerializeField] private GameObject _gameUIPanel;

        private void OnEnable()
        {
            FillInPlayerPanel();
        }
        private void Start()
        {
            _gameUIPanel.SetActive(false);
        }
        private void FillInPlayerPanel()
        {
            for (int i = 0; i < _playerTypes.Players.Count; i++)
            {
                int number = i;
                PlayerSlotView playerSlotView = Instantiate(_playerSlotView, _playerSelectMenu.transform);
                playerSlotView.FillInInfo(_playerTypes.Players[number]);
                playerSlotView.ChooseButton.onClick.AddListener(() =>
                {
                    OnSelectPlayer?.Invoke(_playerTypes.Players[number]);
                    _gameUIPanel.SetActive(true);
                    _playerSelectMenu.SetActive(false);
                });
            }
        }
    }
}