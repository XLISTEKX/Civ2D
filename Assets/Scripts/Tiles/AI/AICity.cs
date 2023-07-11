using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AISystem
{
    public static class AICity
    {
        public static Vector2Int GetProduction(Tile_City city)
        {
            int x = Random.Range(0, 2);
            int y = 0;
            switch (x)
            {
                case 0:
                    y = city.possibleBuildings.Count;
                    if(y == 0)
                    {
                        y = city.possibleUnits.Count;
                        x = 1;
                    } 
                    break;
                case 1:
                    y = city.possibleUnits.Count;
                    break;
            }

            return new(Random.Range(0,y),x);
        }
    }
}

