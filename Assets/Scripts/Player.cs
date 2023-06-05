using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public int money = 0;
    public int science;

    public int ID;

    public List<Unit> allUnits;
    public List<Tile_City> allCities;
    public List<GameObject> possibleBuildings;
    public List<GameObject> possibleUnits;

    public void startNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.nextRound();
        }
        foreach (Tile_City city in allCities)
        {
            money += city.cityResouces.cash;
            science += city.cityResouces.science;
            city.nextTurn();
        }
    }

}
