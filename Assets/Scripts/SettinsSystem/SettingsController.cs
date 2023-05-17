using SettingsSystem;
using System;
using System.Collections.Generic;
using DG.Tweening;
using Tools;

namespace SettingsSystem
{
    internal class SettingsController
    {
        private UIView _uiView;
        private UIModel _uiModel;
        private CoinCounter _coinCounter;
        public SettingsController(UIView uiView)
        {
            _uiView = uiView;
            _uiModel = new UIModel();
            _coinCounter = new CoinCounter();
            UIModel.OnChangeKM += _uiView.SetDistance;
            CoinCounter.OnCollentCoins += _uiView.SetCoinNumber;
        }

        internal UIModel UIModel { get => _uiModel; }
        internal CoinCounter CoinCounter { get => _coinCounter; }
    }

    internal class UIModel 
    {
        public Action<int> OnChangeKM;
        private int _distance = 0;
        private int _distanceSpan;
        public UIModel()
        {
            _distanceSpan = 10;
        }
        public void StartDistanceCount() => CountDistance();

        private void CountDistance()
        {
            Sequence disSequence = DOTween.Sequence();
            disSequence.AppendInterval(Constants.gameMultiplier).AppendCallback(IncreaseDistance).SetLoops(-1); ;
        }
        private void IncreaseDistance()
        {
            _distance += _distanceSpan;
            OnChangeKM?.Invoke(_distance);
        } 
    }

    internal class CoinCounter
    {
        public Action<int> OnCollentCoins;
        private int _coinsCollected;

        public CoinCounter()
        {
            _coinsCollected = 0;
        }

        public void AddCoins(int value)
        {
            _coinsCollected += value;
            OnCollentCoins?.Invoke(_coinsCollected);
        }
    }
}