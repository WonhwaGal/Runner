using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    // Новая фабрика, без лишних объектов с учетом архитектуры проекта
    // Суть фабрик объектов - создавать и отдавать объект, хранить инфу о объектах это не ее функционал
    internal sealed class GenericFactory<T>
        where T: Object
    {
        private const string LoadPath = "{0}/{1}";

        private List<T> _loadedPrefabs;

        public GenericFactory(string folderName)
        {
            ResourcesFolderName = folderName;
            _loadedPrefabs = new List<T>();
        }

        public string ResourcesFolderName { get; }
        public IReadOnlyList<T> LoadedPrefabs => _loadedPrefabs;

        public void LoadPrefab(string prefabName)
        {
            var prefab = Resources.Load<T>(string.Format(LoadPath, ResourcesFolderName, prefabName));
            _loadedPrefabs.Add(prefab);
        }

        public T Create(bool shouldBeSave = false)
        {
            int randomNumber = Random.Range(0, _loadedPrefabs.Count);

            if (shouldBeSave)
                randomNumber = 0;

            return Object.Instantiate(_loadedPrefabs[randomNumber]);
        }
    }
    //*/

    /*
    internal abstract class GenericFactory<T> where T : IRespawnable
    {
        private List<T> _objects;
        protected List<string> _objectNames;
        private string _prefabFolderName;
        private Transform _rootObject;

        public List<T> Objects { get => _objects; }
        public Transform RootObject { get => _rootObject; }

        public GenericFactory(string name)
        {
            _prefabFolderName = name;
            _objects = new List<T>();
            _objectNames = new List<string>();
            _rootObject = new GameObject(name).transform;
        }


        public void AddPrefabNameToList(string name) => _objectNames.Add(name);

        public void CreateListOfObjects()
        {
            for (int i = 0; i < _objectNames.Count; i++)
                CreateSingleObject(i);
        }

        protected GameObject DublicateObject()
        {
            int number = Random.Range(0, _objectNames.Count);
            return CreateSingleObject(number);
        }

        private GameObject CreateSingleObject(int number)
        {
            GameObject prefab = Resources.Load<GameObject>($"{_prefabFolderName}/{_objectNames[number]}");
            GameObject obj
                    = Object.Instantiate(prefab, RootObject);
            obj.SetActive(false);
            Objects.Add(obj.GetComponent<T>());
            return obj;
        }

        public abstract T Spawn();
        public abstract void Despawn(T respawnable);
    }
    //*/
}