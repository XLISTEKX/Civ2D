using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IProduct
{
    public int movementRange;
    public int movementLeft;

    public int productionCost;
    public Sprite unitSprite;

    [SerializeField] Image[] unitColors;  //0 - Out, 1 - In

    public void moveUnit(Tile destination)
    {
        transform.SetParent(destination.transform);
        transform.localPosition = Vector3.zero;
    }
    public void nextRound()
    {
        movementLeft = movementRange;
    }

    public void initUnit(Player player)
    {
        Color color = player.color;
        color.a = 1f;

        unitColors[0].color = color;
        color.a = 0.75f;
        unitColors[1].color = color;
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
