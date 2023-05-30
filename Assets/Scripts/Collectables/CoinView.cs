using UnityEngine;
using DG.Tweening;
using System;

namespace Collectables
{
    internal class CoinView: CollectableObject
    {
        private Vector3 _rotationTargetVector = new Vector3(0, 360, 0);

        private void Start()
        {
            Value = 1;
            Type = CollectableType.Coin;
            Upgrade = UpgradeType.None;
            AnimateCollectable();
        }
        private void OnBecameVisible() => _animationTween.Play();

        public override void PauseAnimation(bool isPaused)
        {
            if (isPaused)
                _animationTween.Pause();
            else
                _animationTween.Play();
        }

        public override void ExecuteAction()
        {
            gameObject.SetActive(false);
            PauseAnimation(true);
        }

        public override void AnimateCollectable()
        {
            _animationTween = transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.LocalAxisAdd)
                .SetLoops(Int32.MaxValue, LoopType.Incremental)
                .SetRelative()
                .SetEase(Ease.Linear);
        }
        private void OnDestroy() => _animationTween.Kill();
    }
}