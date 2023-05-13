using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    internal class TimerController
    {
        private List<Timer> _timers;

        public TimerController()
        {
            Timers = new List<Timer>();
        }

        public List<Timer> Timers { get => _timers; set => _timers = value; }

        public void Record(float deltaTime)
        {
            for (int i = 0; i < Timers.Count; i++)
            {
                if (Timers[i].ShouldBeWorking)
                    Timers[i].Work(deltaTime);
            }
        }
    }
}