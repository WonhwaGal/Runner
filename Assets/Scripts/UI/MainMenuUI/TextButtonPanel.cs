using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

internal class TextButtonPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public event Action<string> OnEnterButton;
    public event Action OnClickButton;

    [SerializeField] private bool _isReactiveButton;
    [SerializeField] private string _triggerName;

    private TextMeshProUGUI _textComponent;
    private Color _initialColor;

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
}
