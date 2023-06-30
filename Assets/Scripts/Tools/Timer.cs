using System;

namespace Tools
{
    internal class Timer
    {
        private bool _onRepeat;
        private float _currentTimePassed;
        private float _targetTimeSpan;

        private Action _action;

        public bool ShouldBeWorking { get; private set; }

        public Timer(Action action, float targetTimeSpan, bool onRepeat, bool startImmediately)
        {
            _action = action;
            _targetTimeSpan = targetTimeSpan;
            _onRepeat = onRepeat;
            CurrentTimeToZero();

            if (startImmediately)
                StartTimer();
        }

        public void Work(float time)
        {
            _currentTimePassed += time;
            if (_currentTimePassed >= _targetTimeSpan)
            {
                _action?.Invoke();
                if (_onRepeat)
                {
                    CurrentTimeToZero();
                }
                else
                    ShouldBeWorking = false;
            }

        }

        private void CurrentTimeToZero() => _currentTimePassed = 0.0f;

        public void StartTimer() => ShouldBeWorking = true;

        public void StopTimer() => ShouldBeWorking = false;
    }
}