using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRespawnable
    {
        Transform RootObject { get; set; }
        GameObject BodyObject { get;  }
        bool IsActive { get; }
        void Activate();
        void Deactivate();
        virtual void PauseChild(bool isPaused) { }
    }
}