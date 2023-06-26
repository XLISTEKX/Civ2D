using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitAiBase : MonoBehaviour
{
    public void MoveUnit()
    {
        Unit unit = GetComponent<Unit>();

        Gameplay_Controler.MoveUnit(GetComponentInParent<Tile>(), UnitAi.GetIdleUnitMove(unit));

    }

}
