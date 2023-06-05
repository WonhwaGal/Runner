using DG.Tweening;
using UnityEngine;

namespace Collectables
{
    internal abstract class CollectableObject : MonoBehaviour
    {
        private int _value;
        protected bool _isMagnetized;
        protected Tween _animationTween;

        public CollectableType Type { get; protected set; }
        public UpgradeType Upgrade { get; protected set; }
        public int Value { get => _value; set => _value = value; }
        public bool IsBeingMagnetized { get => _isMagnetized; }

        public virtual void MoveToTarget(Vector3 position) { }
        public virtual void ReturnToPlace() { }
        public abstract void PauseAnimation(bool isPaused);
        public abstract void ExecuteAction();
        public abstract void AnimateCollectable();
    }
}