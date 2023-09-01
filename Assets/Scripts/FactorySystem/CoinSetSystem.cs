using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class CoinSetSystem: ICoinSetSystem
    {
        private IPool<CoinSetView> _coinPool;
        private Vector3 _yShift = new Vector3(0, 1.5f, 0);

        public CoinSetSystem() => CreateCoinSetFactory();


        private void CreateCoinSetFactory()
        {
            //создаем и настраиваем фабрику объектов
            var coinFactory = new GenericFactory<CoinSetView>("CoinSetPrefabs");
            coinFactory.LoadPrefab("CoinSet1");

            //создаем пулл объектов
            _coinPool = new SingleFactory<CoinSetView>(coinFactory);
        }

        public void UpdateAnimationState(bool isPaused)
        {
            for (int i = 0; i < _coinPool.Objects.Count; i++)
                _coinPool.Objects[i].PauseChild(isPaused);
        }

        public void PutCoinsOnRoad(IRoadSpan roadSpan)
        {
            for (int i = 0; i < roadSpan.CoinSpots.Count; i++)
            {
                CoinSetView coinSetView = _coinPool.Spawn();
                coinSetView.transform.position = roadSpan.CoinSpots[i].position + _yShift;
                coinSetView.transform.rotation = roadSpan.CoinSpots[i].rotation;
                roadSpan.AcceptChildRespawnable(coinSetView, RespawnableType.Coin);
            }
        }
    }
}