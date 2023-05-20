

namespace Collectables
{
    internal interface ICollectable
    {
        CollectableType Type { get; }
        UpgradeType Upgrade { get; }
        int Value { get; }
        void ExecuteAction();
    }

    public enum CollectableType
    {
        None = 0,
        Coin = 1,
        Upgrade = 2,
    }
    public enum UpgradeType
    {
        None = 0,
        Shield = 1,
        DoublePoints = 2,
    }
}