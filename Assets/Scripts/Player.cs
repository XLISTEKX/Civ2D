using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public List<Unit> allUnits;
    public List<Tile_City> allCities;


    public void startNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.nextRound();
        }
    }
}
