using Collectables;
using DG.Tweening;
using System.Collections.Generic;

namespace PlayerSystem
{
    internal class PlayerUpgradeController
    {
        private bool _shieldOn;
        private int _defaultMultiplier;
        private int _coinMultiplier;
        private int _crystalMultiplier;

        private Dictionary<UpgradeType, Sequence> _runningSequences = new();

        public int CoinMultiplier { get => _coinMultiplier; }
        public int CrystalMultiplier { get => _crystalMultiplier;}

        public PlayerUpgradeController()
        {
            _defaultMultiplier = 1;
            _coinMultiplier = _defaultMultiplier;
            _crystalMultiplier = _defaultMultiplier;
        }

        public void ActivateUpgrade(float timeSpan, UpgradeType upgrade)
        {
            if (upgrade == UpgradeType.Shield)
                TurnOnShield(timeSpan);
            else if (upgrade == UpgradeType.DoublePoints)
                SetDoublePoints(timeSpan);

        }
        private void SetDoublePoints(float timeSpan)
        {
            if(_coinMultiplier > _defaultMultiplier)
            {
                KillPreviousSequence(UpgradeType.DoublePoints);
                ResetPoints();
            }

            Sequence doublePointsSequence = DOTween.Sequence();
            doublePointsSequence.AppendCallback(() =>
            {
                _coinMultiplier *= 2;
                _crystalMultiplier *= 2;
            }).AppendInterval(timeSpan)
                .OnComplete(ResetPoints); ;
        }
        private void TurnOnShield(float timeSpan)
        {
            if (_shieldOn)
            {
                KillPreviousSequence(UpgradeType.Shield);
                TurnOffShield();
            }

            Sequence shieldSequence = DOTween.Sequence();
            _runningSequences.Add(UpgradeType.Shield, shieldSequence);
            shieldSequence.AppendCallback(() => _shieldOn = true).AppendInterval(timeSpan)
                    .OnComplete(() => TurnOffShield());
        }

        private void ResetPoints()
        {
            _coinMultiplier = _defaultMultiplier;
            _crystalMultiplier = _defaultMultiplier;
            _runningSequences.Remove(UpgradeType.DoublePoints);
        }
        private void TurnOffShield()
        {
            _shieldOn = false;
            _runningSequences.Remove(UpgradeType.Shield);
        }

        public bool CheckShield()
        {
            if (_shieldOn)
                return true;
            return false;
        }

        private void KillPreviousSequence(UpgradeType upgrade)
        {
            if (_runningSequences.TryGetValue(upgrade, out Sequence sequence))
                sequence.Kill();
        }
    }
}