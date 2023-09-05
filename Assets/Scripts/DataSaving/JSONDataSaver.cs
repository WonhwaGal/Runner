using System.IO;
using UnityEngine;

namespace DataSaving
{
    internal sealed class JSONDataSaver : IDataSaver
    {
        private readonly string _savePath = Application.persistentDataPath + "/DataSaver.json";

        public void Save(ProgressSavedData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(_savePath, json);
        }

        public ProgressSavedData Load()
        {
            ProgressSavedData result = new ProgressSavedData();
            if (!File.Exists(_savePath))
                return result;

            string json = File.ReadAllText(_savePath);
            result = JsonUtility.FromJson<ProgressSavedData>(json);

            return result;
        }
    }
}