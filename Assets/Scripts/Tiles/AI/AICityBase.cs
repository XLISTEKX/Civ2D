using AISystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICityBase : MonoBehaviour
{
    public void GetNextMove()
    {
        Tile_City city = GetComponent<Tile_City>();

        if(city.productionQueue.Count <= 0)
        {
            Vector2Int x = AICity.GetProduction(city);
            city.AddToQueue(x.x, x.y);
        }
    }
}
