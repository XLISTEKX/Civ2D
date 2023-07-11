using UnityEngine;

public class UnitAiBase : MonoBehaviour
{
    bool isIdle = true;

    public virtual void MoveUnit()
    {
        Unit unit = GetComponent<Unit>();
        
        if(isIdle)
            Gameplay_Controler.MoveUnit(GetComponentInParent<Tile>(), UnitAi.GetIdleUnitMove(unit));

    }

}
