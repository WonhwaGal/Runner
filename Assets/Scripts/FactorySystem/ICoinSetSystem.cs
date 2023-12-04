
namespace Factories
{
    internal interface ICoinSetSystem
    {
        void PutCoinsOnRoad(IRoadSpan roadSpan);
        void UpdateAnimationState(bool isPaused);
    }
}