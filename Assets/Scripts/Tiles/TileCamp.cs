using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamp: Tile, ITurnCity, IRenderable
{
    public int cooldown = 1;
    public int maxMobs = 1;

    int maxCooldown;
    [SerializeField] GameObject[] unitPrefabs;
    List<GameObject> unitSpawned = new();
    
    public virtual void StartNextTurn()
    {
        cooldown--;

        if(cooldown <= 0)
        {
            if(unitSpawned.Count < maxMobs)
            {
                SpawnUnit();
                
            }
            cooldown = maxCooldown;
        }
    }

    public void SpawnUnit()
    {
        int t = Random.Range(0, unitPrefabs.Length);
        unitSpawned.Add(GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().SpawnUnit(unitPrefabs[t], this, 1));
        unitSpawned[^1].GetComponent<UnitCamp>().camp = this;

        unitSpawned[^1].GetComponent<UnitCamp>().TurnVisibility(visiblity);
    }

    public void RemoveUnit(GameObject unit)
    {
        unitSpawned.Remove(unit);
    }

    public ResourcesTile GetResources()
    {
        return new(0, 0, 0, 0);
    }

    public override void InitTile(Vector2Int position)
    {
        base.InitTile(position);

        maxCooldown = cooldown;
    }
    public override void TurnRender(bool turn)
    {
        if (discovered)
        {
            GetComponent<SpriteRenderer>().enabled = turn;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = turn;
        }
        else
        {
            GetComponent<SpriteRenderer>().enabled = false;
            transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<TileFog>().TurnFog(turn);
        }
    }
}
