

namespace Collectables
{
    internal interface ICollectable
    {
        CollectableType Type { get; }
        int Value { get; }
        void ExecuteAction();
    }

    public enum CollectableType
    {
        None = 0,
        Coin = 1,
        Upgrade = 2,
    }
}