using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorsInfo : MonoBehaviour
{
    public static Color getColorByGold(int yourCash, int needCash)
    {
        if(yourCash >= needCash)
        {
            return Color.green;
        }
        else
        {
            return Color.red;
        }
    }
}
