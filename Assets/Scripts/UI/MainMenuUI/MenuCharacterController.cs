using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

internal class MenuCharacterController : MonoBehaviour
{
    [SerializeField] private Animator _menuAnimator;

    private const string _sayNoConstant = "SayNO";
    private static readonly int _sayNOHash = Animator.StringToHash(_sayNoConstant);
    private const string _getAngryConstant = "GetAngry";
    private static readonly int _getAngryHash = Animator.StringToHash(_getAngryConstant);

    public void SubscribeToButtons(List<TextButtonPanel> textPanels)
    {
        for (int i = 0; i < textPanels.Count; i++)
            textPanels[i].OnEnterButton += ReceiveButtonEvent;
    }

    public void ReceiveButtonEvent(string eventName)
    {
        if (eventName == _sayNoConstant)
            SayNo();
        else if (eventName == _getAngryConstant)
            GetAngry();
    }

    private void SayNo() => _menuAnimator.SetTrigger(_sayNOHash);
    private void GetAngry() => _menuAnimator.SetTrigger(_getAngryHash);
}
