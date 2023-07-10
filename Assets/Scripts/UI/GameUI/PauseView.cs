using UnityEngine;
using UnityEngine.UI;
using System;
using DG.Tweening;

namespace GameUI
{
    internal class PauseView : MonoBehaviour, IPauseView
    {
        public event Action<bool> OnContinueGame;
        public event Action OnBackToMenu;
        public event Action OnExit;

        [SerializeField] private Button _continue;
        [SerializeField] private Button _backToMenu;
        [SerializeField] private Button _exit;
        [SerializeField] private Transform _root;

        private bool _isPaused;

        public GameObject Gameobject => gameObject;

        public void ShowPauseMenu()
        {
            _isPaused = true;
            _root.rotation = Quaternion.Euler(90, 0, 0);
            _root.DORotate(Vector3.zero, 1.2f);
        }

        public void Init()
        {
            _continue.onClick.AddListener(() => OnContinueGame?.Invoke(!gameObject.activeInHierarchy));
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

        public void HidePauseMenu()
        {
            _isPaused = false;
            _root.DORotate(new Vector3(90, 0, 0), 1.2f).OnComplete(() =>
            {
                if(!_isPaused)
                    gameObject.SetActive(false);
            });
        }


        private void OnDestroy()
        {
            _continue.onClick.RemoveAllListeners();
            _backToMenu.onClick.RemoveAllListeners();
            _exit.onClick.RemoveAllListeners();
        }
    }
}