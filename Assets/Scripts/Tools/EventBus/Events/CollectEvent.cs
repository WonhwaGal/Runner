using Collectables;

public struct CollectEvent : IGameEvent
{
    public readonly CollectableType Type;
    public readonly int Value;

    public CollectEvent(CollectableType type, int value)
    {
        Type = type;
        Value = value;
    }
}