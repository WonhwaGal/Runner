using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeImage : MonoBehaviour
{
    [SerializeField] private Image _mainImage;
    [SerializeField] private Image _timerCircle;

    public Image TimerCircle { get => _timerCircle; }
    public Sprite Sprite { get => _mainImage.sprite; set => _mainImage.sprite = value; }
    public bool IsPaused { get; set; }

    public void StatCountDown(float timeSpan) => StartCoroutine(CountDown(timeSpan));

    private IEnumerator CountDown(float duration)
    {
        float passedTime = 0;
        while(passedTime < duration)
        {
            if (!IsPaused)
            {
                passedTime += Time.deltaTime;
                _timerCircle.fillAmount = Mathf.InverseLerp(0f, 1f, passedTime / duration);
            }
            yield return null;
        }
    }

    public void CancelCountDown() => StopAllCoroutines();
}
