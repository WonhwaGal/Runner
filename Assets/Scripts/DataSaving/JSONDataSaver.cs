using System.IO;
using UnityEngine;

namespace DataSaving
{
    internal sealed class JSONDataSaver : IDataSaver
    {
        private readonly string SavePath = Application.persistentDataPath + "/DataSaver.json";

        public void Save(ProgressSavedData data)
        {
            string json = JsonUtility.ToJson(data);
            File.WriteAllText(SavePath, json);
        }

        public ProgressSavedData Load()
        {
            ProgressSavedData result = new ProgressSavedData();
            if (!File.Exists(SavePath))
                return result;

            string json = File.ReadAllText(SavePath);
            result = JsonUtility.FromJson<ProgressSavedData>(json);

            return result;
        }
    }
}