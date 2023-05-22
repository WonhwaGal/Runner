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
        private int _progressDistance;

        Sequence disSequence;
        public GameUIModel()
        {
            _distanceSpan = 10;
        }
        public void StartDistanceCount() => CountDistance();

        private void CountDistance()
        {
            disSequence = DOTween.Sequence();
            disSequence.AppendInterval(Constants.gameMultiplier)
                .AppendCallback(IncreaseDistance).SetLoops(-1).OnKill(RegisterDistance);
        }
        private void IncreaseDistance()
        {
            _distance += _distanceSpan;
            OnChangeKM?.Invoke(_distance);
        }
        public void StopDistanceCount() => disSequence.Kill();
        
        private void RegisterDistance() => _progressDistance = _distance;
    }
}