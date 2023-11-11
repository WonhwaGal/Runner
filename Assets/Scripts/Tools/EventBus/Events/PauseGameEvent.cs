
public struct PauseGameEvent : IGameEvent
{
    public readonly bool IsPaused;

    public PauseGameEvent(bool value) => IsPaused = value;
}
