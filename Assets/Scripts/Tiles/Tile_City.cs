using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tile_City : Tile, IPointerClickHandler
{
    Player owner;

    public ResourcesTile cityResouces = new ResourcesTile(0,0,0);
    public List<GameObject> buildingsBuild = new List<GameObject>();
    public List<GameObject> productionQueue = new List<GameObject>();
    public List<GameObject> possibleBuildings;
    Gameplay_Controler gameplay_Controler;

    public int buildingProgress;
    public int buildProduction;

    public void initCityTile(Vector2Int position, Player owner)
    {
        this.owner = owner;
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        possibleBuildings = new List<GameObject>(owner.possibleBuildings);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            gameplay_Controler.openCity(this);
        }
            
    }

    public void addToQueue(int id)
    {
        productionQueue.Add(possibleBuildings[id]);
        possibleBuildings.RemoveAt(id);

        if(productionQueue.Count == 1)
        {
            buildProduction = productionQueue[0].GetComponent<Building>().buildCost;
        }

    }
    public void removeFromQueue(int ID)
    {
        possibleBuildings.Add(productionQueue[ID]);
        productionQueue.RemoveAt(ID);
    }
    public void nextTurn()
    {
        if (productionQueue.Count == 0) {
            return;
        }
        int temp = cityResouces.production;

        buildingProgress = buildingProgress + temp;

        if(buildingProgress >= buildProduction)
        {
            buildingsBuild.Add(productionQueue[0]);
            productionQueue.RemoveAt(0);
            buildingsBuild[0].GetComponent<Building>().build(this);

            buildingProgress -= buildProduction;
            buildProduction = productionQueue[0].GetComponent<Building>().buildCost;
        }
    }

    
}
