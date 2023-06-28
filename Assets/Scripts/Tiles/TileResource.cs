using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileResource : Tile
{
    [Space(20)]
    [Header("Resource Settings")]
    [SerializeField] Sprite resourceIcon;
    [SerializeField] UISlotResource slot;


    public override void InitTile(Vector2Int position)
    {
        resources = new ResourcesTile(Random.Range(minResources[0], maxResources[0] + 1), Random.Range(minResources[1], maxResources[1] + 1), Random.Range(minResources[2], maxResources[2] + 1), Random.Range(minResources[3], maxResources[3] + 1));
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        tileCost = ResourcesTile.CalcTileValue(resources * 2);

        maxResources = null;
        minResources = null;

        UpdateUI();
    }

    public override void TurnRender(bool turn)
    {
        if (discovered)
        {
            GetComponent<SpriteRenderer>().enabled = turn;
            slot.TurnVisibility(turn);
        }
        else
        {
            slot.TurnVisibility(false);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<TileFog>().TurnFog(turn);
        }
        
    }

    void UpdateUI()
    {
        slot.InitSlot(resourceIcon);
    }
}
