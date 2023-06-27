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

    public static int GetPopCost()
    {
        return 8;
    }

    public static string[] GetUIResourceValue(ResourcesTile resources)
    {
        string[] output = new string[4];

        output[0] = resources.food.ToString();
        output[1] = resources.production.ToString();
        output[2] = resources.cash.ToString();
        output[3] = resources.science.ToString();

        if (resources.food > 0)
            output[0] = "+" + output[0];
        if (resources.production > 0)
            output[1] = "+" + output[1];
        if (resources.cash > 0)
            output[2] = "+" + output[2];
        if (resources.science > 0)
            output[3] = "+" + output[3];

        return output;
    }

}
