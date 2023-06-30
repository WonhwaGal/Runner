using System;
using static ProgressSystem.GameProgressConfig;


namespace ProgressSystem
{
    internal interface IProgressController: IDisposable
    {
        IGameUIModel GameUIModel { get; }
        ICollectableCounter CollectableCounter { get; }
        GameProgressConfig GameConfig { get; }
        PlayerConfig RecieveCurrentPlayer();
        void RegisterCurrentProgress();
    }
}