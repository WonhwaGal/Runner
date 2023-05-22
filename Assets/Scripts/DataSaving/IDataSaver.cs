

namespace DataSaving
{
    internal interface IDataSaver
    {
        void Save(SavedData data);
        SavedData Load();
    }
}