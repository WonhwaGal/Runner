using System;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal interface IProgressController: IDisposable
    {
        GameUIModel UIModel { get; }
        GameProgressConfig GameConfig { get; }
        CollectableCounter CollectableCounter { get; }
        PlayerConfig RecieveCurrentPlayer();
        void RegisterCurrentProgress();
    }
}