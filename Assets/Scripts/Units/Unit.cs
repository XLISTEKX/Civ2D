using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IProduct, IDamageable
{
    [Header("Health System")]
    
    public int maxHealth;
    [HideInInspector] public int health;
    public int damage;

    [Space(20)]
    [Header("Movement System")]
    public int movementRange;
    [HideInInspector] public int movementLeft;
    public int attackRange;
    [HideInInspector] public bool canAttack = true;

    [Space(20)]
    [Header("Production:")]
    public int productionCost;
    public Sprite unitSprite;
    [Space(20)]
    [Header("Settings")]
    [SerializeField] Image[] unitColors; //0 - Out, 1 - In
    [SerializeField] TMP_Text textHP;
    [HideInInspector] public Player owner;

    [Space(20)]
    [Header("Actions")]
    public UnityEvent[] actions;
    public Sprite[] actionsIcons;
    public void moveUnit(Tile destination)
    {
        transform.SetParent(destination.transform);
        transform.localPosition = Vector3.zero;
    }
    public void nextRound()
    {
        movementLeft = movementRange;
        canAttack = true;
    }

    public virtual void initUnit(Player player)
    {
        health = maxHealth;
        movementLeft = movementRange;
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

    public virtual void KillUnit()
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

        gameplay_Controler.SpawnUnit(gameObject, city, city.owner.ID);
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
            KillUnit();
        }
        updateUI();
    }
}
