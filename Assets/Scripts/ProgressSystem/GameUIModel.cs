using System;
using DG.Tweening;
using Tools;

namespace ProgressSystem
{

    internal class GameUIModel : IGameUIModel
    {
        private int _distance = 0;
        private readonly int _distanceSpan;
        private Sequence _disSequence;

        public GameUIModel() => _distanceSpan = Constants.kmSpan;

        public event Action<int> OnChangeKM;

        public void StartDistanceCount() => CountDistance();

        private void CountDistance()
        {
            _disSequence = DOTween.Sequence();
            _disSequence.AppendInterval(Constants.kmAddTimeSpan)
                .AppendCallback(IncreaseDistance).SetLoops(-1);
        }

        private void IncreaseDistance()
        {
            _distance += _distanceSpan;
            OnChangeKM?.Invoke(_distance);
        }

        public void PauseDistanceCount(bool isPaused)
        {
            if (isPaused)
                _disSequence.Pause();
            else
                _disSequence.Play();
        }

        public void StopDistanceCount() => _disSequence.Kill();

        public void IncreaseSpeed(float timeScale) => _disSequence.timeScale = timeScale;
    }
}