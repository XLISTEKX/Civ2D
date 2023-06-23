using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City 
{
    static readonly string[] cityNames = {"Warsaw", "Berlin", "Pekin", "Paris", "Moskau", "Kiev", "Madrid", "Krakow", 
        "Lublin", "Dresden", "Frankfurt", "Gdansk", "Sydney", "Tokyo", "Washington", "New York", "London"};

    


    public static string randomCityName()
    {
        int random = Random.Range(0, cityNames.Length);

        return cityNames[random];
    }

    public static int turnsToBuild(int production, int buildingProduction)
    {
        return Mathf.CeilToInt(buildingProduction / (float)production);
    }

    

}
