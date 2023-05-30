using GameUI;

namespace Commands
{
    internal class UICommander : ICommander
    {
        private IUiController _uiController;

        internal IUiController UiController { get => _uiController; private set => _uiController = value; }

        public UICommander(IUiController uiController) => UiController = uiController;

        public void Start() => UiController.PauseView.gameObject.SetActive(false);
        public void Pause(bool isPaused) => UiController.PauseGame(isPaused);
        public void Stop() { }
    }
}