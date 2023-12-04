using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal sealed class GenericFactory<T>
        where T : Object
    {
        private const string LoadPath = "{0}/{1}";

        private readonly List<T> _loadedPrefabs;

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

        public T Create(int index) => Object.Instantiate(_loadedPrefabs[index]);
    }
}