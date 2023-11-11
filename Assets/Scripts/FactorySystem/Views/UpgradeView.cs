using Collectables;
using UnityEngine;
using DG.Tweening;
using System;
using System.Collections.Generic;
using Tools;


namespace Factories
{
    internal class UpgradeView : CollectableObject, IRespawnable
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private int _timeActive;

        private bool _isActive;
        private Vector3 _rotationTargetVector = new(0, 360, 0);

        public Transform RootObject { get; set; }
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;

        private void OnBecameVisible() => PauseAnimation(false);
        private void OnBecameInvisible() => PauseAnimation(true);

        private void Start()
        {
            Type = _upgradeType == UpgradeType.Crystal ?
                CollectableType.Crystal : CollectableType.Upgrade;
            Upgrade = _upgradeType;
            Value = _timeActive;
            AnimateCollectable();
        }

        public override void AnimateCollectable()
        {
            _animationTween = transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.LocalAxisAdd)
                .SetLoops(Int32.MaxValue, LoopType.Incremental)
                .SetRelative()
                .SetEase(Ease.Linear);
        }

        public void PauseChild(bool isPaused) => PauseAnimation(isPaused);

        public override void PauseAnimation(bool isPaused)
        {
            if (isPaused)
                _animationTween.Pause();
            else
                _animationTween.Play();
        }

        public void Activate()
        {
            _isActive = true;
            gameObject.SetActive(true);
        }

        public void Deactivate()
        {
            _isActive = false;
            gameObject.transform.SetParent(RootObject);
            gameObject.SetActive(false);
            _animationTween.Pause();

            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
        }

        public override void ExecuteAction()
        {
            gameObject.SetActive(false);
            _isMagnetized = false;
            PauseAnimation(true);
        }

        public override void MoveToTarget(Vector3 position)
        {
            if (!_isMagnetized)
                _isMagnetized = true;
            if (_upgradeType == UpgradeType.Crystal)
                _animationTween = transform.DOMove(position, Constants.coinMagnetSpeed);
        }

        private void OnDestroy() => _animationTween.Kill();
    }
}