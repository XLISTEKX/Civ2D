using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAI : Player
{
    public List<Unit> toDestroy = new();
    public override void StartNextRound()
    {

        foreach (Unit unit in allUnits)
        {
            unit.GetComponent<UnitAiBase>().MoveUnit();
           
            unit.NextRound();
        }
        foreach(Unit unit in toDestroy)
        {
            allUnits.Remove(unit);
            Destroy(unit);
        }

        int tempScience = 0;
        foreach (Tile_City city in allCities)
        {
            ResourcesTile resources = city.GetResources();
            money += resources.cash;
            tempScience += resources.science;
            city.GetComponent<AICityBase>().GetNextMove();
            city.StartNextTurn();
        }
    }
}
