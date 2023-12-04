using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProgressSystem.GameProgressConfig;

namespace GameUI
{
    internal class PlayerSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private Image _image;
        [SerializeField] private Button _chooseButton;
        [SerializeField] private TextMeshProUGUI _buttonText;

        public TextMeshProUGUI Name => _name;
        public Button ChooseButton => _chooseButton;
        public TextMeshProUGUI Description => _description;

        public void FillInInfo(PlayerConfig config)
        {
            Name.text = config.Name;
            Description.text = config.Description;
            _image.sprite = config.PlayerImage;
        }
        
        public void ClosePlayer(PlayerConfig config)
        {
            string currency = config.CurrencyType == CurrencyType.Coins ? "coins" : "crystals"; 
            _buttonText.text = $"{config.CurrencyPrice} {currency}";
            _image.color = Color.black;
        }

        public void OpenPlayer(PlayerConfig config)
        {
            _buttonText.text = "Play";
            _image.color = Color.white;
        }
    }
}