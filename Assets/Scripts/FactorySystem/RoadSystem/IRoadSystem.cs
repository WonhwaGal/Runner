
namespace Factories
{
    internal interface IRoadSystem
    {
        IRoadAnalyzer RoadAnalyzer { get; }
        void UpdatePlayerLane(int number);
        void Dispose();
    }
}