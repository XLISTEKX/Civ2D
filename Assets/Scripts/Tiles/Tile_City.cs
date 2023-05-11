using UnityEngine;


public class Tile_City : Tile
{
    public ResourcesTile cityResouces;
    Gameplay_Controler gameplay_Controler;

    public override void initTile(Vector2Int position)
    {
        this.position = position;
        name = "Tile(" + this.position.x + "," + this.position.y + ")";

        gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();
    }
}
