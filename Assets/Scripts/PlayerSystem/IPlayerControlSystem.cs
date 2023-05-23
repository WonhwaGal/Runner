using System;
using static ProgressSystem.GameProgressConfig;


namespace PlayerSystem
{
    internal interface IPlayerControlSystem : IDisposable
    {
        TriggerHandler TriggerHandler { get; }
        void CreatePlayer(PlayerConfig config);
        void PausePlayer(bool pauseOn);
        void StopPlayer();
    }
}