namespace Factories
{
    internal interface IMainFactory
    {
        IRoadSystem RoadSystem { get; }
        ICoinSetSystem CoinSetSystem { get; }

        void UpdateAnimations(bool isPaused);
        void Dispose();
    }
}