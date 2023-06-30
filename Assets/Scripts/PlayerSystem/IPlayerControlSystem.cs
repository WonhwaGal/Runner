using System;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal interface IPlayerControlSystem : IDisposable
    {
        event Action OnPlayerControllerSet;
        TriggerHandler TriggerHandler { get; }
        IPlayerController PlayerController { get; }
        void CreatePlayer(PlayerConfig config);
        void PausePlayer(bool pauseOn);
        void StopPlayer();
    }
}