using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building_Tile : Tile, IProduct
{
    public int buildingCost;

    public int getBuildCost()
    {
        return buildingCost;
    }
    public Sprite getImage()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }
    public void construct(Tile_City city)
    {
        city.buildLocations[0].Construct();
    }
    public int type()
    {
        return 2;
    }

    public override int getType()
    {
        return 2;
    }
    public override void initTile(Vector2Int position)
    {
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
        updateBorderState();
    }
}
