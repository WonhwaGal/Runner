using UnityEngine;
using DG.Tweening;
using System;

namespace Collectables
{
    internal class CoinView: MonoBehaviour, ICollectable
    {
        private int _value;
        private Sequence _rotationSequence;
        private Vector3 _rotationTargetVector = new Vector3(0,80,0);
        public int Value { get => _value; set => _value = value; }

        public CollectableType Type { get; private set; }
        public UpgradeType Upgrade { get; private set; }

        private void Start()
        {
            _value = 1;
            Type = CollectableType.Coin;
            Upgrade = UpgradeType.None;
            AnimateCollectable();
        }
        private void OnBecameVisible() => _rotationSequence.Play();

        public void ExecuteAction()
        {
            gameObject.SetActive(false);
            _rotationSequence.Pause();
        }

        public void AnimateCollectable()
        {
            _rotationSequence = DOTween.Sequence();
            _rotationSequence.Append(transform.DOLocalRotate(_rotationTargetVector, 1.0f))
                .SetLoops(Int32.MaxValue, LoopType.Yoyo)
                .SetEase(Ease.Linear);
        }
        private void OnDestroy() => _rotationSequence.Kill();
    }
}