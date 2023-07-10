using TMPro;
using UnityEngine;
using DG.Tweening;
using Collectables;
using System.Collections.Generic;

namespace GameUI
{
    internal class GameUIView : MonoBehaviour, IGameUIView
    {
        [SerializeField] private TextMeshProUGUI _coinNumberText;
        [SerializeField] private TextMeshProUGUI _crystalNumberText;
        [SerializeField] private TextMeshProUGUI _distanceKm;

        [Header("Upgrades")]
        [SerializeField] private List<UpgradeImage> _upgradeImages;
        [SerializeField] private List<Sprite> _upgradeSprites;

        private Dictionary<UpgradeType, Sequence> _runningSequences;

        private void Start()
        {
            _coinNumberText.text = "0";
            _distanceKm.text = "0";
            _runningSequences = new Dictionary<UpgradeType, Sequence>();
        }

        public void SetCoinNumber(int number) => _coinNumberText.text = number.ToString();
        public void SetCrystalNumber(int number) => _crystalNumberText.text = number.ToString();
        public void SetDistance(int distance) => _distanceKm.text = distance.ToString();

        public void PauseGame(bool isPaused)
        {
            foreach (var pair in _runningSequences)
            {
                if (isPaused)
                    pair.Value.Pause();
                else
                    pair.Value.Play();
            }
        }

        public void ActivateUpgradeImage(float timeSpan, UpgradeType upgrade)
        {
            Sequence mySequence = DOTween.Sequence();

            if (_runningSequences.ContainsKey(upgrade))
            {
                _runningSequences[upgrade].Kill();
                TurnOffUpgrade(upgrade);
            }
            _runningSequences.Add(upgrade, mySequence);

            mySequence.AppendCallback(() => ChooseUpgradeImage(upgrade, timeSpan))
                .AppendInterval(timeSpan)
                .OnComplete(() => TurnOffUpgrade(upgrade));
        }

        private void ChooseUpgradeImage(UpgradeType upgrade, float timeSpan)
        {
            var result = upgrade switch
            {
                UpgradeType.Shield => _upgradeSprites[(int)UpgradeType.Shield],
                UpgradeType.DoublePoints => _upgradeSprites[(int)UpgradeType.DoublePoints],
                UpgradeType.Magnet => _upgradeSprites[(int)UpgradeType.Magnet],
                _ => _upgradeSprites[(int)UpgradeType.None],
            };

            for(int i = 0; i < _upgradeImages.Count; i++)
            {
                if (_upgradeImages[i].gameObject == null)
                    return;

                if (!_upgradeImages[i].gameObject.activeInHierarchy)
                {
                    _upgradeImages[i].Sprite = result;
                    _upgradeImages[i].gameObject.SetActive(true);
                    _upgradeImages[i].StatCountDown(timeSpan);
                    return;
                }
            }
        }

        private void TurnOffUpgrade(UpgradeType upgrade)
        {
            FindUpgradeImage(upgrade).SetActive(false);
            _runningSequences.Remove(upgrade);
        }

        private GameObject FindUpgradeImage(UpgradeType upgrade)
        {
            for (int i = 0; i < _upgradeImages.Count; i++)
            {
                if (_upgradeImages[i].Sprite == _upgradeSprites[(int)upgrade]
                    && _upgradeImages[i].gameObject.activeInHierarchy)
                {
                    _upgradeImages[i].CancelCountDown();
                    return _upgradeImages[i].gameObject;
                }
            }
            return null;
        }
    }
}