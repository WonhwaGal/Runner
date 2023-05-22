

namespace GameUI
{
    internal interface IUiController
    {
        SelectMenuLogic SelectLogic { get; }
        void PauseGame(bool isPaused);
        void Dispose();
    }
}