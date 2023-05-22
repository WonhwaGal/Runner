using ProgressSystem;
using TMPro;
using UnityEngine;


namespace GameUI
{
    internal class SelectPlayerView : MonoBehaviour
    {
        [SerializeField] private GameObject _playerSelectMenu;
        [SerializeField] private PlayerCfgList _playerTypes;
        [SerializeField] private PlayerSlotView _playerSlotView;
        [SerializeField] private GameObject _gameUIPanel;
        [SerializeField] private TextMeshProUGUI _coinNumber;

        SelectMenuPresenter _presenter;

        public PlayerCfgList PlayerTypes { get => _playerTypes; }

        public void Init(SelectMenuPresenter presenter) => _presenter = presenter;

        public void SetCoinNumber(int number) => _coinNumber.text = number.ToString();
        public void FillInPlayerPanel()
        {
            for (int i = 0; i < PlayerTypes.Players.Count; i++)
            {
                int number = i;
                PlayerSlotView playerSlotView = Instantiate(_playerSlotView, _playerSelectMenu.transform);
                playerSlotView.FillInInfo(PlayerTypes.Players[number]);

                if (PlayerTypes.Players[number].IsOpen)
                    OpenPlayer(playerSlotView, number);
                else
                    ClosePlayer(playerSlotView, number);
            }
        }

        private void OpenPlayer(PlayerSlotView playerSlotView, int number)
        {
            playerSlotView.OpenPlayer(PlayerTypes.Players[number]);
            playerSlotView.ChooseButton.onClick.AddListener(() => OpenPlayerCard(number));
        }

        private void ClosePlayer(PlayerSlotView playerSlotView, int number)
        {
            playerSlotView.ClosePlayer(PlayerTypes.Players[number]);
            playerSlotView.ChooseButton.onClick.AddListener(() => TryToBuyPlayer(playerSlotView, number));
        }

        private void TryToBuyPlayer(PlayerSlotView playerSlotView, int number)
        {
            int totalCoins = int.Parse(_coinNumber.text.ToString());
            if (totalCoins >= PlayerTypes.Players[number].CoinPrice)
            {
                _presenter.BuyPlayer(PlayerTypes.Players[number]);
                playerSlotView.OpenPlayer(PlayerTypes.Players[number]);
                _presenter.OpenNewPlayer(PlayerTypes.Players[number]);

                playerSlotView.ChooseButton.onClick.RemoveAllListeners();
                playerSlotView.ChooseButton.onClick.AddListener(() => OpenPlayerCard(number));
            }
        }
        private void OpenPlayerCard(int number)
        {
            _presenter.AssignSelectedPlayer(PlayerTypes.Players[number]);
            _gameUIPanel.SetActive(true);
            _playerSelectMenu.SetActive(false);
        }
    }
}