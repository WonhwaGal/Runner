using UnityEngine;
using DG.Tweening;
using System;
using Tools;

namespace Collectables
{
    internal class CoinView : CollectableObject
    {
        private Vector3 _rotationTargetVector = new Vector3(0, 360, 0);
        private Vector3 _localPos;

        private void Start()
        {
            Value = 1;
            Type = CollectableType.Coin;
            Upgrade = UpgradeType.None;
            _localPos = transform.localPosition;
            _animationTween = transform.DOMove(transform.position, 0);
            AnimateCollectable();
        }
        private void OnBecameVisible() => AnimateCollectable();

        public override void PauseAnimation(bool isPaused)
        {
            if (isPaused)
                _animationTween.Pause();
            else
                _animationTween.Play();
        }

        public override void MoveToTarget(Vector3 position)
        {
            if (!_isMagnetized)
                _isMagnetized = true;
            _animationTween = transform.DOMove(position, Constants.coinMagnetSpeed);
        }


        public override void ExecuteAction()
        {
            gameObject.SetActive(false);
            _isMagnetized = false;
            PauseAnimation(true);
        }

        public override void AnimateCollectable()
        {
            if (_animationTween != null)
            _animationTween = transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.LocalAxisAdd)
                .SetLoops(Int32.MaxValue, LoopType.Incremental)
                .SetRelative()
                .SetEase(Ease.Linear);
        }

        public override void ReturnToPlace() => transform.localPosition = _localPos;

        private void OnDestroy() => _animationTween.Kill();
    }
}