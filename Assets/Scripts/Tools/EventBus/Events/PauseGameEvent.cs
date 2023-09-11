using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PauseGameEvent : IEvent
{
    public readonly bool GameIsPaused;

    public PauseGameEvent(bool gamePaused) => GameIsPaused = gamePaused;
}
