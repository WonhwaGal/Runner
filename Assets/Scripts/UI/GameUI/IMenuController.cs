

namespace GameUI
{
    internal interface IMenuController
    {
        SelectMenuLogic SelectLogic { get; }
        void Dispose();
    }
}