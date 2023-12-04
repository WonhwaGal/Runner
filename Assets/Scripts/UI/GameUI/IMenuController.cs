using System;

namespace GameUI
{
    internal interface IMenuController : IDisposable
    {
        ISelectLogic SelectLogic { get; }
    }
}