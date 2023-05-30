namespace Commands
{
    internal interface ICommander
    {
        void Start();
        void Pause(bool isPaused);
        void Stop();
    }
}