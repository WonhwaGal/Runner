using System;
using DG.Tweening;
using Tools;

namespace ProgressSystem
{
    internal class GameUIModel 
    {
        public Action<int> OnChangeKM;
        private int _distance = 0;
        private int _distanceSpan;
        private float _increaseSpeed = Constants.increaseSpeedSpan;

        Sequence _disSequence;
        Sequence _speedSequence;
        public GameUIModel()
        {
            _distanceSpan = 10;
        }
        public void StartDistanceCount()
        {
            CountDistance();
            IncreaseCountSpeed();
        }

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

        private void IncreaseCountSpeed()
        {
            _speedSequence = DOTween.Sequence();
            _speedSequence.AppendInterval(_increaseSpeed)
                .AppendCallback(IncreaseSpeed)
                .SetLoops(Int32.MaxValue);
        }
        private void IncreaseSpeed() => _disSequence.timeScale += 0.01f;

        //may need it later to keep track of km
        //private void RegisterDistance() => _progressDistance = _distance;
    }
}