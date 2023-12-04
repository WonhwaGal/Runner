using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal interface IPool<T>
        where T : Object
    {
        IReadOnlyList<T> ActiveObjects { get; }
        IReadOnlyList<T> InactiveObjects { get; }
        Transform RootObject { get; }

        void PrespawnAllPrefabs();
        T Spawn(bool shouldBeSave = false);
        void Despawn(T despawnedObject);
    }
}