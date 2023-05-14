using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile_City : Tile, IPointerClickHandler
{
    public ResourcesTile cityResouces = new ResourcesTile(0,0,0);
    public List<Building> buildingsBuild = new List<Building>();
    public List<Building> productionQueue = new List<Building>();
    Gameplay_Controler gameplay_Controler;

    public override void initTile(Vector2Int position)
    {
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
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
