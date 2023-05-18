using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Unit : MonoBehaviour, IProduct
{
    public int movementRange;
    public int movementLeft;

    public int productionCost;
    public Sprite unitSprite;

    public void moveUnit(Tile destination)
    {
        transform.SetParent(destination.transform);
        transform.localPosition = Vector3.zero;
    }
    public void nextRound()
    {
        movementLeft = movementRange;
    }
    public int getBuildCost()
    {
        return productionCost;
    }
    public Sprite getImage()
    {
        return unitSprite;
    }
    public void construct(Tile_City city)
    {
        Gameplay_Controler gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();

        List<Tile> tiles = gameplay_Controler.findMovesInRange(city, 1).ToList();
        tiles.Remove(city);

        int random = Random.Range(0, tiles.Count);

        gameplay_Controler.spawnUnit(gameObject, tiles[random], city.owner.ID);
    }
    public int type()
    {
        return 1;
    }
}
