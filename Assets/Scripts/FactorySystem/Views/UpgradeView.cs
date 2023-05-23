using Collectables;
using UnityEngine;
using DG.Tweening;
using System;

namespace Factories
{
    internal class UpgradeView : RespawnableObject, ICollectable
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private int _timeActive;

        private Sequence _rotationSequence;
        private Vector3 _rotationTargetVector = new Vector3(0, 360, 0);

        public CollectableType Type { get; private set; }
        public UpgradeType Upgrade { get; private set; }
        public int Value { get; private set; }

        private void Start()
        {
            Type = CollectableType.Upgrade;
            Upgrade = _upgradeType;
            Value = _timeActive;
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
            _rotationSequence.Append(transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.FastBeyond360))
                .SetLoops(6, LoopType.Restart)   
                .SetRelative()
                .SetEase(Ease.Linear);
        }

        private void OnDestroy() => _rotationSequence.Kill();
    }
}