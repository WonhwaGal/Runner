

namespace GameUI
{
    internal interface IUiController
    {
        PauseView PauseView { get; }
        void PauseGame(bool isPaused);
    }
}