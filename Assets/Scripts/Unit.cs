using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IProduct, IDamageable
{
    public int health;
    public int damage;

    public int movementRange;
    public int movementLeft;

    public int productionCost;
    public Sprite unitSprite;

    [SerializeField] Image[] unitColors; //0 - Out, 1 - In
    [SerializeField] TMP_Text textHP;
    public Player owner;

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
        owner = player;
        updateUI();
    }

    void updateUI()
    {
        Color color = owner.color;
        color.a = 1f;

        unitColors[0].color = color;
        color.a = 0.75f;
        unitColors[1].color = color;

        textHP.text = health.ToString();
    }

    void killUnit()
    {
        owner.allUnits.Remove(this);
        Destroy(gameObject);

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

    public void takeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            killUnit();
        }
    }
}
