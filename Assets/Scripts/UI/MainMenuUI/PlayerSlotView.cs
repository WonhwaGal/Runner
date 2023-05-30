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

        public TextMeshProUGUI Name { get => _name; set => _name = value; }
        public Image Image { get => _image; set => _image = value; }
        public Button ChooseButton { get => _chooseButton; set => _chooseButton = value; }
        public TextMeshProUGUI Description { get => _description; set => _description = value; }

        public void FillInInfo(PlayerConfig config)
        {
            Name.text = config.Name;
            Description.text = config.Description;
            _image.sprite = config.PlayerImage;
        }
        
        public void ClosePlayer(PlayerConfig config)
        {
            _buttonText.text = $"Buy {config.CoinPrice} coins";
            _image.color = Color.black;
        }

        public void OpenPlayer(PlayerConfig config)
        {
            _buttonText.text = "Play";
            _image.color = Color.white;
        }
    }
}