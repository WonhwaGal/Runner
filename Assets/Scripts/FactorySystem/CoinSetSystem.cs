using System.Collections.Generic;
using UnityEngine;

namespace Factories
{
    internal class CoinSetSystem: ICoinSetSystem
    {
        private GenericFactory<CoinSetView> _coinFactory;
        private Vector3 _yShift = new Vector3(0, 1.5f, 0);

        public CoinSetSystem() => CreateCoinSetFactory();


        private void CreateCoinSetFactory()
        {
            _coinFactory = new SingleFactory<CoinSetView>("CoinSetPrefabs");
            AddObjectsToTheFactory();
            _coinFactory.CreateListOfObjects();
        }

        private void AddObjectsToTheFactory()
        {
            _coinFactory.AddPrefabNameToList("CoinSet1");
        }

        public void UpdateAnimationState(bool isPaused)
        {
            for (int i = 0; i < _coinFactory.Objects.Count; i++)
                _coinFactory.Objects[i].PauseChild(isPaused);
        }

        public void PutCoinsOnRoad(List<Transform> coinSetSpots)
        {
            for (int i = 0; i < coinSetSpots.Count; i++)
            {
                _coinFactory.Spawn().transform.position = coinSetSpots[i].position + _yShift;
            }
        }
    }
}