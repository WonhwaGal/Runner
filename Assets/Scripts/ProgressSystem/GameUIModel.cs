using System;
using DG.Tweening;
using Tools;

namespace ProgressSystem
{

    internal class GameUIModel : IGameUIModel
    {
        public event Action<int> OnChangeKM;
        private int _distance = 0;
        private int _distanceSpan;
        private float _increaseSpeed = Constants.increaseSpeedSpan;

        Sequence _disSequence;

        public GameUIModel() => _distanceSpan = 10;


        public void StartDistanceCount() => CountDistance();

        private void CountDistance()
        {
            _disSequence = DOTween.Sequence();
            _disSequence.AppendInterval(Constants.gameMultiplier)
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