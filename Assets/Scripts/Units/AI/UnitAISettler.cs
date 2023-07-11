using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAISettler : UnitAiBase
{
    public override void MoveUnit()
    {
        Unit_Settlers unit = GetComponent<Unit_Settlers>();

        Gameplay_Controler.MoveUnit(GetComponentInParent<Tile>(), UnitAi.GetIdleUnitMove(unit));
        unit.createCity();
    }
}
