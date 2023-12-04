using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class SinglePool<T> : IPool<T>
        where T : MonoBehaviour, IRespawnable
    {
        private readonly GenericFactory<T> _factory;
        private readonly List<T> _activeObjects;
        private readonly List<T> _inactiveObjects;

        public SinglePool(GenericFactory<T> factory)
        {
            _factory = factory;
            RootObject = new GameObject(factory.ResourcesFolderName).transform;
            _activeObjects = new List<T>();
            _inactiveObjects = new List<T>();
        }

        public IReadOnlyList<T> ActiveObjects => _activeObjects;
        public IReadOnlyList<T> InactiveObjects => _inactiveObjects;

        public Transform RootObject { get; }

        public T Spawn(bool saveSpawn = false)
        {
            T resultObject;

            if (saveSpawn)
                resultObject = _factory.Create(default);
            else
                resultObject = (InactiveObjects.Count > 0) ? 
                    InactiveObjects[Random.Range(0, InactiveObjects.Count)] : 
                        _factory.Create(Random.Range(0, _factory.LoadedPrefabs.Count));
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

        public void PrespawnAllPrefabs()
        {
            for(int i = 0; i < _factory.LoadedPrefabs.Count; i++)
            {
                var prespawn = _factory.Create(i);
                prespawn.gameObject.SetActive(false);
                _inactiveObjects.Add(prespawn);
                prespawn.RootObject = RootObject;
                prespawn.gameObject.transform.SetParent(RootObject);
            }
        }
    }
}