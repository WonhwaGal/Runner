using Factories;
using System.Collections.Generic;
using UnityEngine;

public class CoinSetSystem
{
    private GenericFactory<CoinSetView> _coinFactory;
    private Vector3 _yShift = new Vector3(0, 1.5f, 0);
    public CoinSetSystem() => CreateCoinSetFactory();

    private void CreateCoinSetFactory()
    {
        _coinFactory = new SingleFactory<CoinSetView>("CoinSetPrefabs");
        _coinFactory.AddPrefabNameToList("CoinSet1");

        _coinFactory.CreateListOfObjects();
    }

    public void PutCoinsOnRoad(List<Transform> coinSetSpots)
    {
        for (int i = 0; i < coinSetSpots.Count; i++)
        {
            _coinFactory.Spawn().transform.position = coinSetSpots[i].position + _yShift;
        }
    }
}
