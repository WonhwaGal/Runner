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
        [SerializeField] private List<CollectableObject> _collectables;
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private int _timeActive;

        private bool _isActive;
        private Tween _rotationTween;
        private Vector3 _rotationTargetVector = new Vector3(0, 360, 0);

        public Transform RootObject { get; set; }
        public GameObject BodyObject => gameObject;
        public bool IsActive => _isActive;
        public List<CollectableObject> Collectables => _collectables;


        private void Start()
        {
            if (_upgradeType == UpgradeType.Crystal)
                Type = CollectableType.Crystal;
            else
                Type = CollectableType.Upgrade;
            Upgrade = _upgradeType;
            Value = _timeActive;
            AnimateCollectable();
        }

        private void OnBecameVisible() => AnimateCollectable();

        public override void AnimateCollectable()
        {
            _rotationTween = transform.DORotate(_rotationTargetVector, 3.0f, RotateMode.LocalAxisAdd)
                .SetLoops(Int32.MaxValue, LoopType.Incremental)
                .SetRelative()
                .SetEase(Ease.Linear);
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
            _rotationTween.Pause();
            if (_collectables.Count > 0)
            {
                for (int i = 0; i < transform.childCount; i++)
                    transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public override void ExecuteAction()
        {
            gameObject.SetActive(false);
            _isMagnetized = false;
            _rotationTween.Pause();
        }

        public override void MoveToTarget(Vector3 position)
        {
            if (!_isMagnetized)
                _isMagnetized = true;
            if (_upgradeType == UpgradeType.Crystal)
                _animationTween = transform.DOMove(position, Constants.coinMagnetSpeed);
        }

        public override void PauseAnimation(bool isPaused)
        {
            if (isPaused)
                _rotationTween.Pause();
            else
                _rotationTween.Play();
        }

        public void PauseChild(bool isPaused)
        {
            for (int i = 0; i < _collectables.Count; i++)
                _collectables[i].PauseAnimation(isPaused);
        }

        private void OnDestroy() => _rotationTween.Kill();
    }
}