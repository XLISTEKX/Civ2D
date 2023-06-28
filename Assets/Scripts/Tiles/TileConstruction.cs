using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileConstruction : Tile
{

    [SerializeField] Sprite[] images;

    GameObject spawn;
    Tile_City city;
    Tile tile;

    public override int getType()
    {
        return 3;
    }

    public void InitConstruction(Vector2Int position, Tile tile)
    {
        discovered = true;

        this.position = position;
        this.tile = tile;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";
        updateBorderState();
    }

    public void setConstruction(GameObject tile, Tile_City city,int ID = 0)
    {
        GetComponent<SpriteRenderer>().sprite = images[ID];
        spawn = tile;
        this.city = city;
    }

    public void Construct()
    {
        Gameplay_Controler gameplay = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
        gameplay.SpawnCityTile(spawn, this, city);


        city.buildLocations.RemoveAt(0);
        Destroy(gameObject);

    }

    public void AbortConstruction()
    {
        tile.gameObject.SetActive(true);
        city.cityTiles.Add(tile);
        city.cityTiles.Remove(this);
        GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Grid_Controler>().tiles[position.x, position.y] = tile;

        Destroy(gameObject);

    }
}
