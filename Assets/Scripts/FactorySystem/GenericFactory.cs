using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal abstract class GenericFactory<T> where T : IRespawnable
    {
        private List<T> objects;
        protected List<string> objectNames;
        private string _prefabFolderName;
        private Transform _rootObject;

        public List<T> Objects { get => objects; private set => objects = value; }

        public GenericFactory(string name)
        {
            _prefabFolderName = name;
            Objects = new List<T>();
            objectNames = new List<string>();
            _rootObject = new GameObject(name).transform;
        }
        public void AddPrefabNameToList(string name) => objectNames.Add(name);

        public abstract T Spawn();

        public void CreateListOfObjects()
        {
            for (int i = 0; i < objectNames.Count; i++)
                CreateSingleObject(i);
        }
        protected GameObject DublicateObject()
        {
            int number = Random.Range(0, objectNames.Count);
            return CreateSingleObject(number);
        }

        private GameObject CreateSingleObject(int number)
        {
            GameObject prefab = Resources.Load<GameObject>($"{_prefabFolderName}/{objectNames[number]}");
            GameObject obj
                    = Object.Instantiate(prefab, _rootObject);
            obj.SetActive(false);
            Objects.Add(obj.GetComponent<T>());
            return obj;
        }
    }
}