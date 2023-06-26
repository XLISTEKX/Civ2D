using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAi : MonoBehaviour
{
    public static Tile GetIdleUnitMove(Unit unit)
    {
        Tile tile = unit.GetComponentInParent<Tile>();

        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        Tile[] returns = gameplay.findMovesInRange(tile, unit.movementLeft);

        return returns[Random.Range(0, returns.Length)];
        
    }
}
