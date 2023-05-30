using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Collectables;
using System.Collections.Generic;

namespace GameUI
{
    public class GameUIView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinNumberText;
        [SerializeField] private TextMeshProUGUI _crystalNumberText;
        [SerializeField] private TextMeshProUGUI _distanceKm;
        [SerializeField] private List<Image> _upgradeImages;
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
            if (isPaused)
            {
                foreach (var pair in _runningSequences)
                    pair.Value.Pause();
            }
            else
            {
                foreach (var pair in _runningSequences)
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

            mySequence.AppendCallback(() => ChooseUpgradeImage(upgrade))
                .AppendInterval(timeSpan)
                .OnComplete(() => TurnOffUpgrade(upgrade));
        }
        private void ChooseUpgradeImage(UpgradeType upgrade)
        {
            var result = upgrade switch
            {
                UpgradeType.Shield => _upgradeSprites[(int)UpgradeType.Shield],
                UpgradeType.DoublePoints => _upgradeSprites[(int)UpgradeType.DoublePoints],
                _ => _upgradeSprites[(int)UpgradeType.None],
            };

            for(int i = 0; i < _upgradeImages.Count; i++)
            {
                if (_upgradeImages[i].gameObject == null)
                    return;
                if (!_upgradeImages[i].gameObject.activeInHierarchy)
                {
                    _upgradeImages[i].sprite = result;
                    _upgradeImages[i].gameObject.SetActive(true);
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
                if (_upgradeImages[i].sprite == _upgradeSprites[(int)upgrade] 
                    && _upgradeImages[i].gameObject.activeInHierarchy)
                    return _upgradeImages[i].gameObject;
            }
            return null;
        }
    }
}