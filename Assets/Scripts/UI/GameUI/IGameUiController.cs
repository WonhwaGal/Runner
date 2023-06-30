
namespace GameUI
{
    internal interface IGameUiController
    {
        IPauseView PauseView { get; }
        void PauseGame(bool isPaused);
    }
}