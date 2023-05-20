using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static ProgressSystem.PlayerCfgList;

namespace GameUI
{
    internal class PlayerSlotView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _name;
        [SerializeField] private Image _image;
        [SerializeField] private Button _chooseButton;

        public TextMeshProUGUI Name { get => _name; set => _name = value; }
        public Image Image { get => _image; set => _image = value; }
        public Button ChooseButton { get => _chooseButton; set => _chooseButton = value; }

        public void FillInInfo(PlayerConfig config)
        {
            Name.text = config.Name;
            Image.sprite = config.Image;
        }
    }
}