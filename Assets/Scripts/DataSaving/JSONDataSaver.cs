using System.IO;
using UnityEngine;

namespace DataSaving
{
    internal sealed class JSONDataSaver : IDataSaver
    {
        private readonly string _savePath = Application.persistentDataPath + "/DataSaver.json";

        public void Save(SavedData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_savePath, json);
        }

        public SavedData Load()
        {
            SavedData result = new SavedData();
            if (!File.Exists(_savePath))
                return result;

            string json = File.ReadAllText(_savePath);
            result = JsonUtility.FromJson<SavedData>(json);

            return result;
        }
    }
}