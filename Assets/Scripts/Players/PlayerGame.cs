using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGame : Player
{
    public override void startNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.nextRound();
        }

        int tempScience = 0;
        foreach (Tile_City city in allCities)
        {
            money += city.cityResouces.cash;
            tempScience += city.cityResouces.science;
            city.nextTurn();
        }

        science = tempScience;

        techtree.nextTurn(science);
    }
}
