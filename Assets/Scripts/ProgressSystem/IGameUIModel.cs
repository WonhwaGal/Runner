using System;

namespace ProgressSystem
{
    internal interface IGameUIModel
    {
        event Action<int> OnChangeKM;
        void StartDistanceCount();
        void PauseDistanceCount(bool isPaused);
        void StopDistanceCount();
        void IncreaseSpeed(float timeScale);
    }
}