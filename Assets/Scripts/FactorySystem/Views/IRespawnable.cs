using Collectables;
using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IRespawnable
    {
        List<CollectableObject> Collectables { get; }
        public GameObject BodyObject { get;  }
        public bool IsActive { get; }
        void Activate();
        void Deactivate();
        virtual void PauseChild(bool isPaused) { }
    }
}