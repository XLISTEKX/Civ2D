using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileCamp: Tile, ITurnCity
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
                cooldown = maxCooldown;
            }
            
        }
    }

    public void SpawnUnit()
    {
        int t = Random.Range(0, unitPrefabs.Length);
        unitSpawned.Add(GameObject.FindGameObjectWithTag("Gameplay").GetComponent<Gameplay_Controler>().SpawnUnit(unitPrefabs[t], this, 1));
        unitSpawned[^1].GetComponent<UnitCamp>().camp = this;
    }

    public void RemoveUnit(GameObject unit)
    {
        unitSpawned.Remove(unit);
    }

    public ResourcesTile GetResources()
    {
        return new(0, 0, 0, 0);
    }

    public override void initTile(Vector2Int position)
    {
        base.initTile(position);

        maxCooldown = cooldown;
    }

}
