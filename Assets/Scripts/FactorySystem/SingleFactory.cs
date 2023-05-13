using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    internal class SingleFactory<T> : GenericFactory<T> where T : RespawnableObject
    {
        private List<T> _inactiveList = new List<T>();
        public SingleFactory(string name) : base(name) { }

        public override T Spawn()
        {
            _inactiveList.Clear();
            for (int i = 0; i < _objects.Count; i++)
            {
                if (!_objects[i].IsActive)
                {
                    _inactiveList.Add(_objects[i]);
                }
            }
            if (_inactiveList.Count > 0)
            {
                int randomNumber = Random.Range(0, _inactiveList.Count);
                _inactiveList[randomNumber].Activate();
                return _inactiveList[randomNumber];
            }
            DublicateObject();
            return Spawn();
        }
    }
}