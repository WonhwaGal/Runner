using Collectables;

public struct UpgradeEvent : IGameEvent
{
    public readonly float Value;
    public readonly UpgradeType UpgradeType;

    public UpgradeEvent(float value, UpgradeType type)
    {
        Value = value;
        UpgradeType = type;
    }
}