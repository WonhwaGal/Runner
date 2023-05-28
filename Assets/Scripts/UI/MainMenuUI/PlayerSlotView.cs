using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProgressSystem.GameProgressConfig;

namespace GameUI
{
    internal class PlayerSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _image;
        [SerializeField] private Button _chooseButton;
        [SerializeField] private TextMeshProUGUI _buttonText;
        [SerializeField] private TextMeshProUGUI _imageText;
        public TextMeshProUGUI Name { get => _name; set => _name = value; }
        public Image Image { get => _image; set => _image = value; }
        public Button ChooseButton { get => _chooseButton; set => _chooseButton = value; }

        public void FillInInfo(PlayerConfig config) => Name.text = config.Name;
        
        public void ClosePlayer(PlayerConfig config)
        {
            _buttonText.text = "Buy";
            _image.sprite = config.CloseImage;
            _imageText.gameObject.SetActive(true);
            _imageText.text = $"You can buy this Player for {config.CoinPrice} coins";
        }

        public void OpenPlayer(PlayerConfig config)
        {
            _buttonText.text = "Play";
            _image.sprite = config.PlayerImage;
            _imageText.gameObject.SetActive(false);
        }
    }
}