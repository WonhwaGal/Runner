using ProgressSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace GameUI
{
    internal class SelectPlayerView : MonoBehaviour
    {
        [SerializeField] private GameObject _playerSelectMenu;
        [SerializeField] private GameProgressConfig _playerTypes;
        [SerializeField] private PlayerSlotView _playerSlotView;
        [SerializeField] private TextMeshProUGUI _coinNumber;
        [SerializeField] private TextMeshProUGUI _crystalNumber;

        private SelectMenuPresenter _presenter;
        private List<PlayerSlotView> _playerSlots;

        public GameProgressConfig PlayerTypes { get => _playerTypes; }

        public void Init(SelectMenuPresenter presenter)
        {
            _presenter = presenter;
            _playerSlots = new();
            FillInPlayerPanel();
        }

        public void SetCoinNumber(int number) => _coinNumber.text = number.ToString();
        public void SetCrystalNumber(int number) => _crystalNumber.text = number.ToString();

        public void FillInPlayerPanel()
        {
            for (int i = 0; i < PlayerTypes.Players.Count; i++)
            {
                PlayerSlotView playerSlotView = Instantiate(_playerSlotView, _playerSelectMenu.transform);
                playerSlotView.FillInInfo(PlayerTypes.Players[i]);
                _playerSlots.Add(playerSlotView);
            }
            UpdatePlayerPanel();
        }
        public void UpdatePlayerPanel()
        {
            for (int i = 0; i < _playerSlots.Count; i++)
            {
                var player = PlayerTypes.Players[i];
                if (player.LimitedUse && player.TimesLeftToPlay == 0)
                    player.IsOpen = false;

                if (player.IsOpen)
                    OpenPlayer(_playerSlots[i], i);
                else
                    ClosePlayer(_playerSlots[i], i);
            }
        }
        private void OpenPlayer(PlayerSlotView playerSlotView, int number)
        {
            playerSlotView.OpenPlayer(PlayerTypes.Players[number]);
            playerSlotView.ChooseButton.onClick.RemoveAllListeners();
            playerSlotView.ChooseButton.onClick.AddListener(() => PlayThisPlayer(number));
        }

        private void ClosePlayer(PlayerSlotView playerSlotView, int number)
        {
            playerSlotView.ClosePlayer(PlayerTypes.Players[number]);
            playerSlotView.ChooseButton.onClick.RemoveAllListeners();
            playerSlotView.ChooseButton.onClick.AddListener(() => TryToBuyPlayer(playerSlotView, number));
        }

        private void TryToBuyPlayer(PlayerSlotView playerSlotView, int number)
        {
            int totalCoins = int.Parse(_coinNumber.text.ToString());
            if (totalCoins >= PlayerTypes.Players[number].CoinPrice)
            {
                _presenter.BuyPlayer(PlayerTypes.Players[number]);
                playerSlotView.OpenPlayer(PlayerTypes.Players[number]);

                playerSlotView.ChooseButton.onClick.RemoveAllListeners();
                playerSlotView.ChooseButton.onClick.AddListener(() => PlayThisPlayer(number));
            }
        }
        private void PlayThisPlayer(int number) 
            => _presenter.MakePlayerCurrent(PlayerTypes.Players[number]);
    }
}