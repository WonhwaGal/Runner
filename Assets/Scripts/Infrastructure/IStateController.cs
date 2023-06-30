namespace Infrastructure
{
    internal interface IStateController
    {
        void PauseGame(bool isPaused);
        void LoseGame();
        void Dispose();
    }
}