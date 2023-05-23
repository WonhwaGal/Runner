using System;
using static ProgressSystem.GameProgressConfig;

namespace ProgressSystem
{
    internal interface IProgressController: IDisposable
    {
        GameUIModel UIModel { get; }
        GameProgressConfig GameConfig { get; }
        CoinCounter CoinCounter { get; }
        PlayerConfig RecieveCurrentPlayer();
        void RecieveCurrentProgress();
    }
}