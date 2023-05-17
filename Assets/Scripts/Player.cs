using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;

    public List<Unit> allUnits;
    public List<Tile_City> allCities;
    public List<GameObject> possibleBuildings;

    public void startNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.nextRound();
        }
        foreach (Tile_City city in allCities)
        {
            city.nextTurn();
        }
    }

}
