using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Color color;
    public int money = 0;
    public int science;

    public int ID;

    public List<Unit> allUnits;
    public List<Tile_City> allCities;
    public List<GameObject> possibleBuildings;
    public List<GameObject> possibleUnits;

    public Techtree techtree;
    public virtual void StartNextRound()
    {
        if(allUnits.Count != 0)
        {
            foreach (Unit unit in allUnits)
            {
                unit.GetComponent<UnitAiBase>().MoveUnit();
                unit.NextRound();
            }
        }
        

        int tempScience = 0;
        foreach (Tile_City city in allCities)
        {
            ResourcesTile resources = city.GetResources();
            money += resources.cash;
            tempScience += resources.science;
            city.StartNextTurn();
        }
    }



    public void updateTech(TechNode tech)
    {
        foreach(GameObject unit in tech.unlockUnit)
        {
            foreach(Tile_City city in allCities)
            {
                city.possibleUnits.Add(unit);
            }
        }
        foreach (GameObject building in tech.unlockBuildings)
        {
            foreach (Tile_City city in allCities)
            {
                city.possibleBuildings.Add(building);
            }
        }
    }
}
