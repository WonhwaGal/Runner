using UnityEngine;
using DG.Tweening;

namespace Collectables
{
    internal abstract class CollectableObject : MonoBehaviour
    {
        private int _value;
        protected bool _isMagnetized;
        protected Tween _animationTween;

        public CollectableType Type { get; protected set; }
        public UpgradeType Upgrade { get; protected set; } = UpgradeType.None;
        public int Value { get => _value; set => _value = value; }
        public bool IsBeingMagnetized => _isMagnetized;

        public abstract void AnimateCollectable();
        public abstract void PauseAnimation(bool isPaused);
        public abstract void ExecuteAction();
        public virtual void MoveToTarget(Vector3 position) { }
        public virtual void ReturnToPlace() { }
    }
}