using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGame : Player
{
    public override void StartNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.nextRound();
        }

        int tempScience = 0;
        foreach (Tile_City city in allCities)
        {
            ResourcesTile resources = city.GetResources();
            money += resources.cash;
            tempScience += resources.science;
            city.StartNextTurn();
        }

        science = tempScience;

        techtree.nextTurn(science);
    }
}
