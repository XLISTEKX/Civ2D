using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_City : Tile, IPointerClickHandler
{
    Player owner;

    public ResourcesTile cityResouces = new ResourcesTile(0,0,0);
    public List<GameObject> buildingsBuild = new List<GameObject>();
    public List<GameObject> productionQueue = new List<GameObject>();
    public List<GameObject> possibleBuildings;
    Gameplay_Controler gameplay_Controler;

    public void initCityTile(Vector2Int position, Player owner)
    {
        this.owner = owner;
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        possibleBuildings = owner.possibleBuildings;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (canClick)
        {
            gameplay_Controler.openCity(this);
        }
            
    }

    public void addToQueue()
    {

    }
}
