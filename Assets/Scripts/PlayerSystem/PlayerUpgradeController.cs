using UnityEngine;
using Collectables;
using DG.Tweening;
using System.Collections.Generic;

namespace PlayerSystem
{
    internal class PlayerUpgradeController
    {
        private GameObject _shield;
        private GameObject _magnet;
        private bool _shieldOn;
        private readonly int _defaultMultiplier;
        private int _coinMultiplier;
        private int _crystalMultiplier;
        private PlayerMagnetController _playerMagnetController;

        private readonly Dictionary<UpgradeType, Sequence> _runningSequences = new ();

        public int CoinMultiplier { get => _coinMultiplier; }
        public int CrystalMultiplier { get => _crystalMultiplier; }

        public PlayerUpgradeController()
        {
            _defaultMultiplier = 1;
            _coinMultiplier = _defaultMultiplier;
            _crystalMultiplier = _defaultMultiplier;
        }

        public void AddComponents(
            PlayerMagnetController playerMagnetController, GameObject shield, GameObject magnet)
        {
            _playerMagnetController = playerMagnetController;
            _shield = shield;
            _magnet = magnet;
        }

        public void PauseUpgradeViews(bool pauseOn)
        {
            foreach (var sequence in _runningSequences.Values)
            {
                if (pauseOn)
                    sequence.Pause();
                else
                    sequence.Play();
            }
        }

        public void ActivateUpgrade(float timeSpan, UpgradeType upgrade)
        {
            if (upgrade == UpgradeType.Shield)
                TurnOnShield(timeSpan);
            if (upgrade == UpgradeType.DoublePoints)
                SetDoublePoints(timeSpan);
            if (upgrade == UpgradeType.Magnet)
                DrewCoins(timeSpan);
        }

        private void DrewCoins(float timeSpan)
        {
            if (_playerMagnetController.gameObject.activeInHierarchy)
            {
                KillPreviousSequence(UpgradeType.Magnet);
                UnDrewCoins();
            }

            Sequence magnetSequence = DOTween.Sequence();
            _runningSequences.Add(UpgradeType.Magnet, magnetSequence);
            magnetSequence.AppendCallback(() =>
            {
                _playerMagnetController.gameObject.SetActive(true);
                _magnet.SetActive(true);
            })
                .AppendInterval(timeSpan)
                .OnComplete(UnDrewCoins);
        }

        private void UnDrewCoins()
        {
            if (_playerMagnetController != null)
            {
                _playerMagnetController.gameObject.SetActive(false);
                _magnet.SetActive(false);
                _runningSequences.Remove(UpgradeType.Magnet);
            }
        }

        private void SetDoublePoints(float timeSpan)
        {
            if (_coinMultiplier > _defaultMultiplier)
            {
                KillPreviousSequence(UpgradeType.DoublePoints);
                ResetPoints();
            }

            Sequence doublePointsSequence = DOTween.Sequence();
            _runningSequences.Add(UpgradeType.DoublePoints, doublePointsSequence);
            doublePointsSequence.AppendCallback(() =>
            {
                _coinMultiplier *= 2;
                _crystalMultiplier *= 2;
            })
                .AppendInterval(timeSpan)
                .OnComplete(ResetPoints);
        }

        private void ResetPoints()
        {
            _coinMultiplier = _defaultMultiplier;
            _crystalMultiplier = _defaultMultiplier;
            _runningSequences.Remove(UpgradeType.DoublePoints);
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
            shieldSequence.AppendCallback(() =>
            {
                _shield.SetActive(true);
                _shieldOn = true;
            }).AppendInterval(timeSpan)
                    .OnComplete(() => TurnOffShield());
        }

        private void TurnOffShield()
        {
            _shield.SetActive(false);
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