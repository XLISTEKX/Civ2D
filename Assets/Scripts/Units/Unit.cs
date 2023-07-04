using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Unit : MonoBehaviour, IProduct, IDamageable, ISeeable
{
    [Header("Health System")]
    
    public int maxHealth;
    [HideInInspector] public int health;
    public int damage;

    [Space(20)]
    [Header("Movement System")]
    public int movementRange;
    public int viewRange;
    [HideInInspector] public int movementLeft;
    public int attackRange;
    [HideInInspector] public bool canAttack = true;

    [Space(20)]
    [Header("Production:")]
    public int productionCost;
    public Sprite unitSprite;
    [Space(20)]
    [Header("Settings")]
    [SerializeField] Image unitIcon;
    [SerializeField] Slider healthBar;
    [HideInInspector] public Player owner;

    [Space(20)]
    [Header("Actions")]
    public UnityEvent[] actions;
    public Sprite[] actionsIcons;

    [HideInInspector] public Tile[] tilesInRange;
    public virtual void MoveUnit(Tile destination)
    {
        transform.SetParent(destination.transform);
        transform.localPosition = Vector3.zero;

        if(owner.ID != 0)
            GetAITilesInRange();
    }

    public void GetAITilesInRange()
    {
        Gameplay_Controler gameplay = Gameplay_Controler.GetControler();
        tilesInRange = gameplay.FindTilesInRange(GetComponentInParent<Tile>(), viewRange);
    }


    public void NextRound()
    {
        movementLeft = movementRange;
        canAttack = true;
    }

    public virtual void InitUnit(Player player)
    {
        health = maxHealth;
        movementLeft = movementRange;
        owner = player;
        UpdateUI();
    }

    void UpdateUI()
    {
        Color color = owner.color;

        unitIcon.color = color;

        healthBar.value = health / (float) maxHealth;
        healthBar.fillRect.GetComponent<Image>().color = ColorsInfo.GetColorByHealth(health, maxHealth);
    }

    public virtual void KillUnit()
    {
        owner.allUnits.Remove(this);
        Destroy(gameObject);

    }

    public int GetBuildCost()
    {
        return productionCost;
    }
    public Sprite GetImage()
    {
        return unitSprite;
    }
    public void Construct(Tile_City city)
    {
        Gameplay_Controler gameplay_Controler = GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>();

        gameplay_Controler.SpawnUnit(gameObject, city, city.owner.ID);
    }
    public int type()
    {
        return 1;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if(health <= 0)
        {
            KillUnit();
        }
        if(GetComponent<SpriteRenderer>().enabled)
            UpdateUI();
    }

    public void TurnVisibility(bool turn)
    {
        GetComponent<SpriteRenderer>().enabled = turn;
        transform.GetChild(0).gameObject.SetActive(turn);
        UpdateUI();
    }

    #region Seeable

    public Tile[] GetTilesInRange()
    {
        return tilesInRange;
    }

    public void SetTilesInRange(Tile[] tiles)
    {
        tilesInRange = tiles;
    }

    #endregion
}
