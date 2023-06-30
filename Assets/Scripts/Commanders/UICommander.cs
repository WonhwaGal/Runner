using GameUI;

namespace Commands
{
    internal class UICommander : ICommander
    {
        private IGameUiController _uiController;

        internal IGameUiController UiController { get => _uiController; private set => _uiController = value; }

        public UICommander(IGameUiController uiController) => UiController = uiController;


        public void Start() => UiController.PauseView.Gameobject.SetActive(false);
        public void Pause(bool isPaused) => UiController.PauseGame(isPaused);
        public void Stop() { }
    }
}