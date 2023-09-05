

namespace DataSaving
{
    internal interface IDataSaver
    {
        void Save(ProgressSavedData data);
        ProgressSavedData Load();
    }
}