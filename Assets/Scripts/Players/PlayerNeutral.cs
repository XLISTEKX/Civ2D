using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNeutral : Player
{
    public List<TileCamp> camps = new();

    public override void StartNextRound()
    {
        foreach (Unit unit in allUnits)
        {
            unit.GetComponent<UnitAiBase>().MoveUnit();
            unit.NextRound();
        }
        foreach (TileCamp camp in camps)
        {
            camp.StartNextTurn();
        }
    }
}
