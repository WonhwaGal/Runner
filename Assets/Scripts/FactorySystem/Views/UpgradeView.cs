using Collectables;
using UnityEngine;


namespace Factories
{
    internal class UpgradeView : RespawnableObject, ICollectable
    {
        [SerializeField] private UpgradeType _upgradeType;
        [SerializeField] private int _timeActive;

        public CollectableType Type { get; private set; }
        public UpgradeType Upgrade { get; private set; }
        public int Value { get; private set; }

        private void Start()
        {
            Type = CollectableType.Upgrade;
            Upgrade = _upgradeType;
            Value = _timeActive;
        }

        public void ExecuteAction() => gameObject.SetActive(false);
    }
}