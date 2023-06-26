using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCamp : Unit
{
    public TileCamp camp;

    public override void KillUnit()
    {
        camp.RemoveUnit(gameObject);

        base.KillUnit();
    }
}
