using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamp : Unit
{
    
    public TileCamp camp;

    public override void MoveUnit(Tile destination)
    {
        base.MoveUnit(destination);

        TurnVisibility(GetComponentInParent<Tile>().visiblity);
    }

    public override void KillUnit()
    {
        if(camp != null)
            camp.RemoveUnit(gameObject);

        base.KillUnit();
    }

}
