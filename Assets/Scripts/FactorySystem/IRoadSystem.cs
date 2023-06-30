using System;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRoadSystem
    {
        event Action<List<Transform>> OnRoadForCoins;
        event Action<List<Transform>> OnRoadForUpdates;

        void CheckPlayerLane(int number);
        //void IncreaseSpeed(float timeScale);
        //void StartRoadSpawn();
        //void PauseRoadSpawn();
        //void StopRoadSpawn();
        void Dispose();
    }
}