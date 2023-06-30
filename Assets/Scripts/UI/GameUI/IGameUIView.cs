using Collectables;

namespace GameUI
{
    internal interface IGameUIView
    {
        public void SetCoinNumber(int number);
        public void SetCrystalNumber(int number);
        public void SetDistance(int distance);
        void PauseGame(bool isPaused);
        void ActivateUpgradeImage(float timeSpan, UpgradeType upgrade);
    }
}