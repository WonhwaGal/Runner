﻿using System;
using UnityEngine;
using DG.Tweening;
using Tools;

namespace Collectables
{
    internal class CoinView : CollectableObject
    {
        private Vector3 _rotationTargetVector = new (0, 360, 0);
        private Vector3 _localPos;

        private void Start()
        {
            Value = 1;
            Type = CollectableType.Coin;
            _localPos = transform.localPosition;
            _animationTween = transform.DOMove(transform.position, 0);
            AnimateCollectable();
        }

        private void OnBecameVisible() => PauseAnimation(false);
        private void OnBecameInvisible() => PauseAnimation(true);

        public override void AnimateCollectable()
        {
            _animationTween = transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.LocalAxisAdd)
                .SetLoops(Int32.MaxValue, LoopType.Incremental)
                .SetRelative()
                .SetEase(Ease.Linear);
        }

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

        public override void ReturnToPlace() => transform.localPosition = _localPos;

        private void OnDestroy() => _animationTween.Kill();
    }
}