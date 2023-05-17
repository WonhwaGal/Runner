using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coinNumberText;
    [SerializeField] private TextMeshProUGUI _distanceKm;
    [SerializeField] private Image _upgradeImage;


    private void Start()
    {
        _coinNumberText.text = "0";
        _distanceKm.text = "0";
    }
    public void SetCoinNumber(int number) => _coinNumberText.text = number.ToString();
    
    public void SetDistance(int distance) => _distanceKm.text = distance.ToString();
}
