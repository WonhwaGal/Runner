using System;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

internal class TextButtonPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IDisposable
{
    [SerializeField] private bool _isReactiveButton;
    [SerializeField] private string _triggerName;
    private TextMeshProUGUI _textComponent;
    private Color _initialColor;

    public event Action<string> OnEnterButton;
    public event Action OnClickButton;

    private void Start()
    {
        _textComponent = GetComponent<TextMeshProUGUI>();
        _initialColor = _textComponent.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _textComponent.DOColor(Color.yellow, 0.5f);
        if (_isReactiveButton)
            OnEnterButton?.Invoke(_triggerName);
    }

    public void OnPointerExit(PointerEventData eventData) => _textComponent.DOColor(_initialColor, 0.5f);

    public void OnPointerClick(PointerEventData eventData) => OnClickButton?.Invoke();

    public void Dispose()
    {
        OnEnterButton = null;
        OnClickButton = null;
    }
}