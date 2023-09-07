using System.IO;
using UnityEngine;

namespace DataSaving
{
    internal sealed class JSONDataSaver : IDataSaver
    {
        private readonly string SavePath = Application.persistentDataPath + "/DataSaver.json";

        public void Save(SavedData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public SavedData Load()
        {
            SavedData result = new SavedData();
            if (!File.Exists(SavePath))
                return result;

            string json = File.ReadAllText(SavePath);
            result = JsonUtility.FromJson<SavedData>(json);

            return result;
        }
    }
}