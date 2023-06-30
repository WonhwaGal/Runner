

namespace GameUI
{
    internal interface IMenuController
    {
        ISelectLogic SelectLogic { get; }
        void Dispose();
    }
}