using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    internal interface IPool<T>
        where T : Object
    {
        IReadOnlyList<T> Objects { get; }
        Transform RootObject { get; }
        T Spawn();
        void Despawn(T despawnedObject);
    }

    //True name SinglePool or ObjectPool
    // Этот объект хранит инфу о созданных объектах на фабрике, вызвается из систем для строики уровня - это чистый пул объектов
    internal class SingleFactory<T> : IPool<T>
        where T : MonoBehaviour, IRespawnable
    {
        private readonly GenericFactory<T> Factory;

        private List<T> _objects;

        public SingleFactory(GenericFactory<T> factory)
        {
            Factory = factory;
            RootObject = new GameObject(factory.ResourcesFolderName).transform;

            _objects = new List<T>();
        }

        public IReadOnlyList<T> Objects => _objects;
        public Transform RootObject { get; }

        public T Spawn()
        {
            var resultObject = (Objects.Count > 0) ? Objects[default] : Factory.Create();
            _objects.Remove(resultObject);
            OnSpawn(resultObject);

            return resultObject;
        }

        public void Despawn(T despawnObject)
        {
            OnDespawn(despawnObject);
            _objects.Add(despawnObject);
        }

        protected virtual void OnSpawn(in T spawnedObject)
        {
            spawnedObject.gameObject.SetActive(true);
            spawnedObject.Activate();
            spawnedObject.RootObject = RootObject;
        }

        protected virtual void OnDespawn(in T desapawnObject)
        {
            desapawnObject.gameObject.SetActive(false);
        }
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
    }
    //*/
}