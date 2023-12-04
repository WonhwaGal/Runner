using GameUI;

namespace Commands
{
    internal class UICommander : ICommander
    {
        private GameUIController _uiController;

        internal GameUIController UiController { get => _uiController; private set => _uiController = value; }

        public UICommander(GameUIController uiController) => UiController = uiController;


        public void Start() => UiController.PauseView.Gameobject.SetActive(false);

        public void Pause(bool isPaused) => UiController.PauseGame(isPaused);

        public void Stop() { }
    }
}