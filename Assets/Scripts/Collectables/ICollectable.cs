

namespace Collectables
{
    internal interface ICollectable
    {
        CollectableType Type { get; }
        UpgradeType Upgrade { get; }
        int Value { get; }
        void ExecuteAction();
        void AnimateCollectable();
    }
}