using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
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
    }
}