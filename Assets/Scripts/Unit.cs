using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour, ISelectable
{
    public int movementRange;
    public int movementLeft;

    public void moveUnit(Tile destination)
    {
        transform.SetParent(destination.transform);
        transform.localPosition = Vector3.zero;
    }
}
