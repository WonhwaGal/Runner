using System;
using static ProgressSystem.GameProgressConfig;


namespace ProgressSystem
{
    internal interface IProgressController: IDisposable
    {
        IGameUIModel GameUIModel { get; }
        CollectableCounter CollectableCounter { get; }
        GameProgressConfig GameConfig { get; }
        PlayerConfig RecieveCurrentPlayer();
        void RegisterCurrentProgress();
    }
}