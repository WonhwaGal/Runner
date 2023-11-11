using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    internal interface IPool<T>
        where T : Object
    {
        IReadOnlyList<T> ActiveObjects { get; }
        IReadOnlyList<T> InactiveObjects { get; }
        Transform RootObject { get; }
        T Spawn(bool shouldBeSave = false);
        void Despawn(T despawnedObject);
    }
    //*/
    /*
    internal class SingleFactory<T> : GenericFactory<T> where T : IRespawnable
    {
        private List<T> _inactiveList = new List<T>();

        public SingleFactory(string name) : base(name) { }


        public override T Spawn()
        {
            _inactiveList.Clear();
            for (int i = 0; i < Objects.Count; i++)
            {
                if (!Objects[i].IsActive)
                    _inactiveList.Add(Objects[i]);
            }
            if (_inactiveList.Count > 0)
            {
                int randomNumber = Random.Range(0, _inactiveList.Count);
                _inactiveList[randomNumber].Activate();
                _inactiveList[randomNumber].RootObject = RootObject;
                return _inactiveList[randomNumber];
            }
            DublicateObject();

            return Spawn();
        }

        public override void Despawn(T respawnable) => respawnable.Deactivate();
    }
    //*/
}