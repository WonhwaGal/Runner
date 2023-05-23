using UnityEngine;
using UnityEngine.UI;
using System;

public class PauseView : MonoBehaviour
{
    public Action OnContinueGame;
    public Action OnBackToMenu { get; set; }
    public Action OnExit;

    [SerializeField] private Button _continue;
    [SerializeField] private Button _backToMenu;
    [SerializeField] private Button _exit;

    public void Init()
    {
        _continue.onClick.AddListener(() => OnContinueGame?.Invoke());
        _backToMenu.onClick.AddListener(() => OnBackToMenu?.Invoke());
        _exit.onClick.AddListener(() => OnExit?.Invoke());
        _exit.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPlaying)
            UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
        Application.Quit();
#endif
    }
    private void OnDestroy()
    {
        _continue.onClick.RemoveAllListeners();
        _backToMenu.onClick.RemoveAllListeners();
        _exit.onClick.RemoveAllListeners();
    }
}
