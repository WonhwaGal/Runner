using UnityEngine;
using System.Collections.Generic;

namespace Factories
{
    //True name SinglePool or ObjectPool
    // Этот объект хранит инфу о созданных объектах на фабрике, вызвается из систем для строики уровня - это чистый пул объектов
    internal class SinglePool<T> : IPool<T>
        where T : MonoBehaviour, IRespawnable
    {
        private readonly GenericFactory<T> Factory;
        private readonly List<T> _activeObjects;
        private readonly List<T> _inactiveObjects;

        public SinglePool(GenericFactory<T> factory)
        {
            Factory = factory;
            RootObject = new GameObject(factory.ResourcesFolderName).transform;
            _activeObjects = new List<T>();
            _inactiveObjects = new List<T>();
        }

        public IReadOnlyList<T> ActiveObjects => _activeObjects;
        public IReadOnlyList<T> InactiveObjects => _inactiveObjects;

        public Transform RootObject { get; }

        public T Spawn(bool shouldBeSave = false)
        {
            T resultObject;

            if (shouldBeSave)
                resultObject = Factory.Create(true);
            else
                resultObject = (InactiveObjects.Count > 0) ? 
                    InactiveObjects[Random.Range(0, InactiveObjects.Count)] : Factory.Create();
            _activeObjects.Add(resultObject);
            _inactiveObjects.Remove(resultObject);

            OnSpawn(resultObject);
            return resultObject;
        }

        public void Despawn(T despawnObject)
        {
            _inactiveObjects.Add(despawnObject);
            _activeObjects.Remove(despawnObject);
            OnDespawn(despawnObject);
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

        public override void Despawn(T respawnable) => respawnable.Deactivate();
    }
    //*/
}